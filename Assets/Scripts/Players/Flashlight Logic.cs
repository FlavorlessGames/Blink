using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FlashlightLogic : NetworkBehaviour
{
    // public AudioClip flashlightFocus;
    // public AudioClip flashlightWiden;
    // public AudioClip flashlightClick;
    [SerializeField] private Light _flashlight;
    [SerializeField] private float _flickerInterval = .2f;
    [SerializeField] private float _maxBattery = 50f;
    [SerializeField] private float _lowBattery = 10f;

    [SerializeField] private float _focusAngle = 30f;
    [SerializeField] private float _focusRange = 85f;

    [SerializeField] private float _wideAngle = 100f;
    [SerializeField] private float _wideRange = 20f;
    [SerializeField] private float _lerpDuration = .3f;
    private bool _isOn  
    {
        get { return IsOwner ? _localOn : _networkOn.Value; }
        set 
        {
            _localOn = value;
            setOnRpc(value);
        }
    }
    private bool _localOn;  // Todo: This value isn't syncing correctly
    private NetworkVariable<bool> _networkOn = new NetworkVariable<bool>();
    private float _currentBattery 
    {
        get { return IsOwner ? _localBattery : _networkBattery.Value; }
        set 
        {
            _localBattery = value;
            setBatteryRpc(value);
        }
    }
    private float _localBattery;
    private NetworkVariable<float> _networkBattery = new NetworkVariable<float>();
    private float _currentAngle 
    {
        get { return IsOwner ? _localAngle : _networkAngle.Value; }
        set 
        {
            _localAngle = value;
            setAngleRpc(value);
        }
    }
    private float _localAngle;
    private NetworkVariable<float> _networkAngle = new NetworkVariable<float>();
    private float _currentRange 
    {
        get { return IsOwner ? _localRange : _networkRange.Value; }
        set 
        {
            _localRange = value;
            setRangeRpc(value);
        }
    }
    private float _localRange;
    private NetworkVariable<float> _networkRange = new NetworkVariable<float>();
    private bool _isFlickering = false;
    private Coroutine _lerpCoroutine;

    private void Start()
    {
        _currentBattery = _maxBattery;
        _currentRange = _wideRange;
        _currentAngle = _wideAngle;
    }

    public override void OnNetworkSpawn() 
    {
        if (!IsOwner) return;
        _isOn = true;
    }

    private void Update()
    {
        UpdateLight();
        lightRayCast();
        if (!IsOwner) return;
        batteryLevel();
        if (PauseManager.Instance.IsPaused) return;
        if (Input.GetButtonDown("Flashlight Button")) lightSwitch();
        if (Input.GetButtonDown("Flashlight Focus")) flashlightFocus();
        if (Input.GetButtonUp("Flashlight Focus")) flashlightUnfocus();
    }

    [Rpc(SendTo.Server)]
    private void setBatteryRpc(float batteryValue)
    {
        _networkBattery.Value = batteryValue;
    }

    [Rpc(SendTo.Server)]
    private void setOnRpc(bool onValue)
    {
        _networkOn.Value = onValue;
    }

    [Rpc(SendTo.Server)]
    private void setRangeRpc(float rangeValue)
    {
        _networkRange.Value = rangeValue;
    }

    [Rpc(SendTo.Server)]
    private void setAngleRpc(float angleValue)
    {
        _networkAngle.Value = angleValue;
    }

    private void flashlightUnfocus()
    {
        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpLight(_wideAngle, _wideRange));
    }

    private void flashlightFocus()
    {
        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpLight(_focusAngle, _focusRange));
    }

    private void batteryLevel()
    {
        if (!_isOn) return;
        if(_currentBattery > 0f)
        {
            _currentBattery -= Time.deltaTime;

            HUDManager.Instance.SetBatteryLevel(100 * _currentBattery / _maxBattery);

            if((_currentBattery < _lowBattery) && !_isFlickering)
            {
                StartFlicker();
            }
            return;
        }
        _isOn = false;
    }

    private void lightSwitch()
    {
        // AudioManager.instance.PlaySound(flashlightClick);
        _isOn = !_isOn;
    }

    IEnumerator Flicker()
    {
        _isFlickering = true;
        while (_isFlickering)
        {
            lightSwitch(); // Toggle the light on/off
            // AudioManager.instance.PlaySound(flashlightClick);
            yield return new WaitForSeconds(_flickerInterval);
        }
    }

    private void StartFlicker()
    {
        StartCoroutine(Flicker());
    }

    public void StopFlicker()
    {
        StopCoroutine(Flicker());
        _isFlickering = false;
    }

    public void ChargeBattery()
    {
        _currentBattery = _maxBattery;
        StopFlicker();
        _isOn = true;
    }

    private IEnumerator LerpLight(float targetAngle, float targetRange)
    {
        float initialAngle = _currentAngle;
        float initialRange = _currentRange;
        float elapsedTime = 0f;

        while (elapsedTime < _lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _lerpDuration;

            _currentAngle = Mathf.Lerp(initialAngle, targetAngle, t);
            _currentRange = Mathf.Lerp(initialRange, targetRange, t);

            yield return null;
        }

        _currentAngle = targetAngle;
        _currentRange = targetRange;
    }

    private void UpdateLight()
    {
        _flashlight.enabled = _isOn;
        _flashlight.spotAngle = _currentAngle;
        _flashlight.range = _currentRange;
    }

    // Todo: Separate this so it can apply to other light sources
    private void lightRayCast()
    {
        if (!_flashlight.enabled) return;
        if (EntityManager.Instance == null) return;
        foreach(Vector3 enemyPosition in EntityManager.Instance.GetEnemyPositions())
        {
            detectWithLight(enemyPosition);
        }
    }

    private void detectWithLight(Vector3 enemyPosition)
    {
        if (!inFlashlightCone(enemyPosition)) return;
        LightDetection detectable = rayCastCheck(enemyPosition);
        if (detectable == null) return;
        detectable.Spotted();
    }

    private bool inFlashlightCone(Vector3 enemyPosition)
    {
        if (Vector3.Distance(_flashlight.transform.position, enemyPosition) > _currentRange) return false;
        Vector3 point1 = _flashlight.transform.forward;
        Vector3 point2 = enemyPosition - _flashlight.transform.position;
        if (Vector3.Angle(point1, point2) > _flashlight.spotAngle / 2f) return false;
        return true;
    }

    private LightDetection rayCastCheck(Vector3 enemyPosition)
    {
        Vector3 direction = (enemyPosition - _flashlight.transform.position).normalized;
        Ray ray = new Ray(_flashlight.transform.position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            LightDetection lightDetection = hit.transform.GetComponent<LightDetection>();
            return lightDetection;
        }
        return null;
    }
}
