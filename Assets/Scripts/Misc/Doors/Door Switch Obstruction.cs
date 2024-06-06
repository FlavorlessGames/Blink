using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Door))]
public class DoorSwitchObstruction : MonoBehaviour
{
    [SerializeField] private List<Lock> _locks;
    [SerializeField] private int _requiredLocks = 1;
    private Dictionary<Lock, bool> _lockState;
    private Door _door;
    void Start()
    {
        _lockState = new Dictionary<Lock, bool>();
        _door = GetComponent<Door>();
        foreach (Lock l in _locks)
        {
            l.LockOpen += lockOpen;
            _lockState[l] = false;
        }
    }

    private void lockOpen(Lock l)
    {
        _lockState[l] = true;
        if (checkOpen()) _door.Open();
    }

    private bool checkOpen()
    {
        int count = 0;
        foreach (bool open in _lockState.Values)
        {
            count += open ? 1 : 0;
            if (count >= _requiredLocks) return true;
        }
        return false;
    }
}