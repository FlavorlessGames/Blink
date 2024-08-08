using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(DoorMovement))]
public class DoorControl : MonoBehaviour
{
    [SerializeField] private List<SwitchGroup> _switchGroups;
    private DoorMovement _door;

    void Start()
    {
        _door = GetComponent<DoorMovement>();
        Debug.Assert(_door != null);
        Debug.Assert(_switchGroups.Count > 0);
        foreach (SwitchGroup sg in _switchGroups)
        {
            sg.Init();
            sg.Flipped += checkOpenSwitches;
            sg.Disable();
        }
        _switchGroups[0].Enable();
    }

    private void checkOpenSwitches()
    {
        bool enable = false;
        foreach (SwitchGroup sg in _switchGroups)
        {
            if (sg.Open)
            {
                enable = true;
            }
            else if (enable)
            {
                sg.Enable();
                enable = false;
                _door.Close();
                return;
            }
            else
            {
                _door.Close();
                return;
            }
        }

        _door.Open();
    }
}