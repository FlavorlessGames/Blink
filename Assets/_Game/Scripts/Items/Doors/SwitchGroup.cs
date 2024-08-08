using UnityEngine;
using System.Collections.Generic;

public enum SwitchGroupType
{
    Any,
    All,
    Count,
}

[System.Serializable]
public class SwitchGroup
{
    [SerializeField]
    private List<DoorSwitch> _switches = new();
    public event FlippedSwitchGroupHandler Flipped;
    [SerializeField]
    private SwitchGroupType _type;
    private List<DoorSwitch> _openSwitches = new();
    public bool Open { get { return openCheck(); } }
    public int Count = 1;

    public SwitchGroup()
    {
        _switches = new List<DoorSwitch>();
    }

    public void Enable()
    {
        setEnabled(true);
    }

    public void Disable()
    {
        setEnabled(false);
    }

    private void setEnabled(bool val)
    {
        foreach(DoorSwitch ds in _switches)
        {
            if (val) ds.Enable();
            else ds.Disable();
        }
    }

    public void Init()
    {
        foreach (DoorSwitch ds in _switches)
        {
            ds.Flipped += switchFlipped;
        }
    }

    private void switchFlipped(DoorSwitch ds)
    {
        if (ds.Open) switchOpened(ds);
        else switchClosed(ds);
    }

    private void switchOpened(DoorSwitch ds)
    {
        if (_openSwitches.Contains(ds)) return;
        _openSwitches.Add(ds);
        updateState();
    }
    
    private void switchClosed(DoorSwitch ds)
    {
        if (!_openSwitches.Contains(ds)) return;
        _openSwitches.Remove(ds);
        updateState();
    }

    private void updateState()
    {
        Flipped?.Invoke();
    }

    private bool openCheck()
    {
        switch (_type)
        {
            case SwitchGroupType.Any:
                return (_openSwitches.Count > 0);
            case SwitchGroupType.All:
                return (_openSwitches.Count >= _switches.Count);
            case SwitchGroupType.Count:
                return (_openSwitches.Count >= Count);
            default:
                Debug.LogError("Switch Group Type Not Supported");
                return false;
        }
    }
}

public delegate void FlippedSwitchGroupHandler();