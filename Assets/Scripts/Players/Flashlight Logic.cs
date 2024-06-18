using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(FlashlightStats))]
public class FlashlightLogic : NetworkBehaviour
{
    // public AudioClip flashlightFocus;
    // public AudioClip flashlightWiden;
    // public AudioClip flashlightClick;
    [SerializeField] private Light _flashlight;
    [SerializeField] private Camera _cam;
    [SerializeField] private PositionalAudio _audio;
    private bool _flickering = false;
    private float _flickerTimer;
    private int _batteryPacks = 0;
    private Coroutine _lerpCoroutine;
    private FlashlightStats _stats;
    private void Start()
    {
        _stats = GetComponent<FlashlightStats>();
    }

    public override void OnNetworkSpawn() 
    {
        if (!IsOwner) return;
        _stats.TurnedOn = true;
    }

    private void Update()
    {
        UpdateLight();
        castFocusedBeam();
        if (!IsOwner) return;
        batteryLevel();
        if (_flickering) flicker();
        if (PauseManager.Instance.IsPaused) return;
        if (Input.GetButtonDown("Flashlight Button")) lightSwitch();
        if (Input.GetButtonDown("Flashlight Focus")) flashlightFocus();
        if (Input.GetButtonUp("Flashlight Focus")) flashlightUnfocus();
        if (Input.GetButtonDown("Recharge")) chargeBattery();
    }


    private void flashlightUnfocus()
    {
        _audio.Play("Flashlight Unfocus");
        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpLight(_stats.WideAngle, _stats.WideRange));
    }

    private void flashlightFocus()
    {
        _audio.Play("Flashlight Focus");
        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpLight(_stats.FocusAngle, _stats.FocusRange));
    }

    private void batteryLevel()
    {
        if (!_stats.TurnedOn) return;
        if (outOfBattery()) return;
        updateBatteryLevel();
        checkFlickering();
    }

    private void checkFlickering()
    {
        if (_flickering) return;
        if (_stats.CurrentBattery < _stats.LowBattery) _flickering = true;
    }

    private bool outOfBattery()
    {
        if (_stats.CurrentBattery > 0) return false;
        _stats.ForcedOff = true;
        _stats.CurrentBattery = 0f;
        return true;
    }

    private void updateBatteryLevel()
    {
        _stats.CurrentBattery -= Time.deltaTime;
        HUDManager.Instance.SetBatteryLevel(100 * _stats.CurrentBattery / _stats.MaxBattery);
    }

    private void lightSwitch()
    {
        _audio.Play("Flashlight Click");
        if (_stats.CurrentBattery < 0) return;
        _stats.TurnedOn = !_stats.TurnedOn;
    }

    private void flicker()
    {
        _flickerTimer -= Time.deltaTime;
        if (_flickerTimer > 0) return;
        _stats.ForcedOff = !_stats.ForcedOff;
        _flickerTimer = _stats.FlickerInterval;
    }

    public void PickupBattery()
    {
        HUDManager.Instance.AddBatteryPack();
        _batteryPacks++;
    }

    private void chargeBattery()
    {
        if (!sufficientBatteryPacksCheck()) return;
        _batteryPacks--;
        _stats.CurrentBattery = _stats.MaxBattery;
        _flickering = false;
        _stats.ForcedOff = false;
    }

    private bool sufficientBatteryPacksCheck()
    {
        if (_batteryPacks > 0) return true;
        return false;
        // Use this function to setup a debug
    }

    private IEnumerator LerpLight(float targetAngle, float targetRange)
    {
        float initialAngle = _stats.CurrentAngle;
        float initialRange = _stats.CurrentRange;
        float elapsedTime = 0f;

        while (elapsedTime < _stats.LerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _stats.LerpDuration;

            _stats.CurrentAngle = Mathf.Lerp(initialAngle, targetAngle, t);
            _stats.CurrentRange = Mathf.Lerp(initialRange, targetRange, t);

            yield return null;
        }

        _stats.CurrentAngle = targetAngle;
        _stats.CurrentRange = targetRange;
    }

    private void UpdateLight()
    {
        _flashlight.enabled = _stats.On;
        _flashlight.spotAngle = _stats.CurrentAngle;
        _flashlight.range = _stats.CurrentRange;
    }

    private void castFocusedBeam()
    {
        if (!_stats.On) return;
        if (_stats.CurrentAngle != _stats.FocusAngle) return;

        var screenCenter = new Vector3(Screen.width/2, Screen.height/2, 0);        
        Ray ray = _cam.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            FocusBeamReceiver receiver = hit.collider.gameObject.GetComponent<FocusBeamReceiver>();
            if (receiver != null && Utility.InRange(hit.point, transform.position, receiver.Range))
            {
                receiver.Target();
                return;
            }
        }
    }
}
