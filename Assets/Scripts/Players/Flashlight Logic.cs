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
    [SerializeField] private PositionalAudio _audio;
    private bool _on  
    {
        get 
        { 
            if (_forcedOff) return false;
            return _turnedOn;
        }
        set { _turnedOn = value; }
    }
    private bool _turnedOn  
    {
        get { return IsOwner ? _localTurnedOn : _networkTurnedOn.Value; }
        set 
        {
            _localTurnedOn = value;
            setTurnedOnRpc(value);
        }
    }
    private bool _localTurnedOn;
    private NetworkVariable<bool> _networkTurnedOn = new NetworkVariable<bool>();
    private bool _forcedOff  
    {
        get { return IsOwner ? _localForcedOff : _networkForcedOff.Value; }
        set 
        {
            _localForcedOff = value;
            setForcedOffRpc(value);
        }
    }
    private bool _localForcedOff;
    private NetworkVariable<bool> _networkForcedOff = new NetworkVariable<bool>();
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
    private bool _flickering = false;
    private float _flickerTimer;
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
        _turnedOn = true;
    }

    private void Update()
    {
        UpdateLight();
        lightRayCast();
        if (!IsOwner) return;
        batteryLevel();
        if (_flickering) flicker();
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
    private void setTurnedOnRpc(bool onValue)
    {
        _networkTurnedOn.Value = onValue;
    }

    [Rpc(SendTo.Server)]
    private void setForcedOffRpc(bool onValue)
    {
        _networkForcedOff.Value = onValue;
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
        _audio.Play("Flashlight Unfocus");
        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpLight(_wideAngle, _wideRange));
    }

    private void flashlightFocus()
    {
        _audio.Play("Flashlight Focus");
        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpLight(_focusAngle, _focusRange));
    }

    private void batteryLevel()
    {
        if (!_turnedOn) return;
        if (outOfBattery()) return;
        updateBatteryLevel();
        checkFlickering();
    }

    private void checkFlickering()
    {
        if (_flickering) return;
        if (_currentBattery < _lowBattery) _flickering = true;
    }

    private bool outOfBattery()
    {
        if (_currentBattery > 0) return false;
        _forcedOff = true;
        _currentBattery = 0f;
        return true;
    }

    private void updateBatteryLevel()
    {
        _currentBattery -= Time.deltaTime;
        HUDManager.Instance.SetBatteryLevel(100 * _currentBattery / _maxBattery);
    }

    private void lightSwitch()
    {
        _audio.Play("Flashlight Click");
        if (_currentBattery < 0) return;
        _turnedOn = !_turnedOn;
    }

    private void flicker()
    {
        _flickerTimer -= Time.deltaTime;
        if (_flickerTimer > 0) return;
        _forcedOff = !_forcedOff;
        _flickerTimer = _flickerInterval;
    }

    public void ChargeBattery()
    {
        _currentBattery = _maxBattery;
        _flickering = false;
        _forcedOff = false;
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
        _flashlight.enabled = _on;
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
