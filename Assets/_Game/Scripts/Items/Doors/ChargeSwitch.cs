
using UnityEngine;

[RequireComponent(typeof(DoorSwitch))]
[RequireComponent(typeof(FocusBeamReceiver))]
public class ChargeSwitch: MonoBehaviour
{
    void Start()
    {
        FocusBeamReceiver rec = GetComponent<FocusBeamReceiver>();
        rec.FullChargeEvent += charged;
    }

    private void charged()
    {
        Debug.Log("Charged");
        DoorSwitch doorSwitch = GetComponent<DoorSwitch>();
        doorSwitch.Invert();
    }
}