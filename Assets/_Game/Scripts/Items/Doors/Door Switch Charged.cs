using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Door))]
public class DoorSwitchCharged : MonoBehaviour
{
    [SerializeField] private List<FocusBeamReceiver> _switches;
    private Door _door;
    void Start()
    {
        _door = GetComponent<Door>();
        foreach (FocusBeamReceiver i in _switches)
        {
            i.FullChargeEvent += doorSwitch;
        }
    }

    private void doorSwitch()
    {
        _door.OpenClose();
    }
}