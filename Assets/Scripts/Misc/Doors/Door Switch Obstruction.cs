using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

[RequireComponent(typeof(Door))]
public class DoorSwitchObstruction : NetworkBehaviour
{
    [SerializeField] private List<Lock> _locks;
    [SerializeField] private int _requiredLocks = 1;
    private bool [] _lockStates;
    private Door _door;
    void Start()
    {
        _door = GetComponent<Door>();
        _lockStates = new bool [_locks.Count];
        int index = 0;
        foreach (Lock l in _locks)
        {
            l.LockOpen += lockOpen;
            l.Index = index;
            _lockStates[index] = false;
            index ++;
        }
    }

    private void lockOpen(Lock l)
    {
        setLockStateRpc(l.Index, true);
        if (checkOpen()) _door.Open();
    }

    [Rpc(SendTo.Everyone)]
    private void setLockStateRpc(int i, bool state)
    {
        _lockStates[i] = state;
    }

    private bool checkOpen()
    {
        int count = 0;
        foreach (bool open in _lockStates)
        {
            count += open ? 1 : 0;
            if (count >= _requiredLocks) return true;
        }
        return false;
    }
}