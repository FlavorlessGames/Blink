using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    public event FlippedSwitchHandler Flipped;
    public bool Open => _open;
    private bool _open = false;
    public bool StuckOpen = false;
    public bool Enabled => _enabled;
    private bool _enabled = true;

    public void Enable()
    {
        _enabled = true;
    }

    public void Disable()
    {
        _enabled = false;
    }

    public void OpenSwitch()
    {
        if (!_enabled) return;
        _open = true;
        invokeEvent();
    }

    public void CloseSwitch()
    {
        if (!_enabled) return;
        if (StuckOpen) return;
        _open = false;
        invokeEvent();
    }

    public void Invert()
    {
        if (!_enabled) return;
        _open = StuckOpen ? true : !_open;
        invokeEvent();
    }

    private void invokeEvent()
    {
        Flipped?.Invoke(this);
    }
}

public delegate void FlippedSwitchHandler(DoorSwitch ds);
