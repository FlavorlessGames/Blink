using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(FlashlightStats))]
public class FlashlightLogic : NetworkBehaviour
{
    [SerializeField] private Light _flashlight;
    [SerializeField] private Camera _cam;
    [SerializeField] private PositionalAudio _audio;
    public bool DebugFlag = false;
    private bool _flickering = false;
    private float _flickerTimer;
    private int _batteryPacks = 0;
    private Coroutine _lerpCoroutine;
    private Coroutine _chargingCoroutine;
    private FlashlightStats _stats;
    private float _focusTimer = 0f;
    public GameObject BatteryPackPrefab;

    private void Start()
    {
        _stats = GetComponent<FlashlightStats>();
        FlameColors.Standard = _flashlight.color;
    }

    public override void OnNetworkSpawn() 
    {
        if (!IsOwner) return;
        _stats = GetComponent<FlashlightStats>();
        _stats.TurnedOn = true;
    }

    private void Update()
    {
        UpdateLight();
        castFocusedBeam();
        if (!IsOwner) return;

        updateFocusTimer();
        batteryLevel();
        secondaryBatteryLevel();
        if (_flickering) flicker();
        if (PauseManager.Instance.IsPaused) return;

        if (Input.GetButtonDown("Flashlight Button")) lightSwitch();
        if (Input.GetButtonDown("Flashlight Focus")) flashlightFocus();
        if (Input.GetButtonUp("Flashlight Focus")) flashlightUnfocus();
        if (Input.GetButtonDown("Recharge")) chargeBattery();
        if (Input.GetButtonUp("Recharge")) cancelCharge();
        if (Input.GetButtonDown("Drop")) dropHeldBattery();
        if (Input.GetButtonDown("Swap Flame")) swapFlame();
    }

    private void swapFlame()
    {
        Debug.Log("Swapping Flame");
        _stats.SecondaryActive = !_stats.SecondaryActive;
        if (_stats.SecondaryActive) _flashlight.color = FlameColors.GetColor(_stats.FlameColor);
        else _flashlight.color = FlameColors.Standard;
    }

    private void flashlightUnfocus()
    {
        _audio.Play("Flashlight Unfocus");
        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpLight(_stats.WideAngle, _stats.WideRange));
        if (!focusOnCooldown()) startFocusCooldown();
    }

    private void flashlightFocus()
    {
        if (focusOnCooldown()) return;
        _audio.Play("Flashlight Focus");
        if (_lerpCoroutine != null) StopCoroutine(_lerpCoroutine);
        _lerpCoroutine = StartCoroutine(LerpLight(_stats.FocusAngle, _stats.FocusRange));
    }

    private bool focusOnCooldown()
    {
        if (DebugFlag) return false;
        return _focusTimer > 0;
    }

    private void startFocusCooldown()
    {
        _focusTimer = _stats.FocusCooldown;
    }

    private void updateFocusTimer()
    {
        if (_focusTimer < 0) return;
        _focusTimer -= Time.deltaTime;
    }

    private void batteryLevel()
    {
        if (!_stats.On) return;
        if (outOfBattery()) return;

        updateBatteryLevel();
        checkFlickering();
    }

    private void secondaryBatteryLevel()
    {
        if (!_stats.On || !_stats.SecondaryActive) return;
        HUDManager.Instance.SetSecondaryBatteryLevel(100 * _stats.SecondaryBattery / _stats.MaxBattery);
        if (_stats.SecondaryActive) _stats.SecondaryBattery -= Time.deltaTime;
        if (_stats.SecondaryBattery > 0) return;
        swapFlame();
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

    public bool PickupBattery()
    {
        if (_batteryPacks >= _stats.MaxBatteryCarryingCapacity) return false;
        _batteryPacks++;
        HUDManager.Instance.SetBatteryPackCount(_batteryPacks);
        return true;
    }

    public void dropHeldBattery()
    {
        if (_batteryPacks <= 0) return;

        _batteryPacks--;
        HUDManager.Instance.SetBatteryPackCount(_batteryPacks);
        Vector3 placement = transform.position + transform.forward * 2 + transform.up;
        GameObject go = Instantiate(BatteryPackPrefab, placement, new Quaternion(0,0,0,0));
        NetworkObject no = go.GetComponent<NetworkObject>();
        go.GetComponent<BatteryPack>().Toss(_flashlight.transform.forward);
        no.Spawn();
    }

    private void chargeBattery()
    {
        Debug.Log("charge");
        if (!sufficientBatteryPacksCheck() && !DebugFlag) return;
        cancelCharge();
        _chargingCoroutine = StartCoroutine(chargeTimer());
    }

    private void cancelCharge()
    {
        if (_chargingCoroutine == null) return;

        StopCoroutine(_chargingCoroutine);
        HUDManager.Instance.SetChargeBar(false, 0f);
        _stats.ForcedOff = false;  // This may create issues
    }

    private IEnumerator chargeTimer()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _stats.ReplaceBatteryDuration)
        {
            _stats.ForcedOff = true;
            elapsedTime += Time.deltaTime;
            HUDManager.Instance.SetChargeBar(true, elapsedTime/_stats.ReplaceBatteryDuration);
            yield return null;
        }

        finishCharge();
        yield return null;
    }

    private void finishCharge()
    {
        _batteryPacks--;
        HUDManager.Instance.SetBatteryPackCount(_batteryPacks);
        HUDManager.Instance.SetChargeBar(false, 0f);
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
