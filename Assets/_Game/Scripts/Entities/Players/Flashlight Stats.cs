using Unity.Netcode;
using UnityEngine;

public class FlashlightStats : NetworkBehaviour
{
    public PlayerMovement PlayerController;

    public float FlickerInterval = .2f;
    public float MaxBattery = 50f;
    public float LowBattery = 10f;

    public float FocusAngle = 30f;
    public float FocusRange = 85f;

    [Range(0f, .5f)]
    public float FocusCooldown = 0.5f;

    public float WideAngle = 100f;
    public float WideRange = 20f;
    public float LerpDuration = .3f;
    public float ReplaceBatteryDuration = 1f;
    public int MaxBatteryCarryingCapacity = 3;
    public bool On  
    {
        get 
        { 
            if (ForcedOff) return false;
            if (PlayerController.Running) return false;
            return TurnedOn;
        }
        set { TurnedOn = value; }
    }
    public bool TurnedOn  
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
    public bool ForcedOff  
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
    public float CurrentBattery 
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
    public float SecondaryBattery 
    {
        get { return IsOwner ? _localSecondaryBattery : _networkSecondaryBattery.Value; }
        set 
        {
            _localSecondaryBattery = value;
            setSecondaryBatteryRpc(value);
        }
    }
    private float _localSecondaryBattery;
    private NetworkVariable<float> _networkSecondaryBattery = new NetworkVariable<float>();
    public bool SecondaryActive 
    {
        get { return IsOwner ? _localSecondaryActive: _networkSecondaryActive.Value; }
        set
        {
            _localSecondaryActive= value;
            setSecondaryActiveRpc(value);
        }
    }
    private bool _localSecondaryActive;
    private NetworkVariable<bool> _networkSecondaryActive= new NetworkVariable<bool>();
    public float CurrentAngle 
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
    public float CurrentRange 
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
    public FlameColor FlameColor
    {
        get { return IsOwner ? _localColor : _networkColor.Value; }
        set
        {
            _localColor = value;
            setColorRpc(value);
        }
    }
    private FlameColor _localColor;
    private NetworkVariable<FlameColor> _networkColor = new NetworkVariable<FlameColor>();

    void Start()
    {
        CurrentAngle = WideAngle;
        CurrentBattery = MaxBattery;
        CurrentRange = WideRange;
        SecondaryBattery = MaxBattery;
    }

    [Rpc(SendTo.Server)]
    private void setColorRpc(FlameColor fc)
    {
        _networkColor.Value = fc;
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

    [Rpc(SendTo.Server)]
    private void setSecondaryActiveRpc(bool flt)
    {
        _networkSecondaryActive.Value = flt;
    }

    [Rpc(SendTo.Server)]
    private void setSecondaryBatteryRpc(float batteryValue)
    {
        _networkSecondaryBattery.Value = batteryValue;
    }
}