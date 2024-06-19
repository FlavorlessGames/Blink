using Unity.Netcode;

public class FlashlightStats : NetworkBehaviour
{

    public float FlickerInterval = .2f;
    public float MaxBattery = 50f;
    public float LowBattery = 10f;

    public float FocusAngle = 30f;
    public float FocusRange = 85f;

    public float WideAngle = 100f;
    public float WideRange = 20f;
    public float LerpDuration = .3f;
    public float ReplaceBatteryDuration = 1f;
    public bool On  
    {
        get 
        { 
            if (ForcedOff) return false;
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

    void Start()
    {
        CurrentAngle = WideAngle;
        CurrentBattery = MaxBattery;
        CurrentRange = WideRange;
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
}