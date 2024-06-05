using UnityEngine;

[RequireComponent(typeof(FocusBeamReceiver))]
[RequireComponent(typeof(Rotator))]
public class ChargeCube : MonoBehaviour
{
    private FocusBeamReceiver _receiver;
    private Rotator _rotator;

    void Start()
    {
        _receiver = GetComponent<FocusBeamReceiver>();
        _rotator = GetComponent<Rotator>();
    }

    void Update()
    {
        _rotator.SetSpeed(_receiver.ChargePercentage);
    }
}