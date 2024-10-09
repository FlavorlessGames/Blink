using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TransformLerper))]
public class StatueTrap : MonoBehaviour
{
    [SerializeField] private bool _armed = true;
    public event TrapTriggerHandler TrapTriggered;
    [SerializeField] private Transform _statueLockPosition;
    private StatueBase _lockedStatue;

    void OnTriggerEnter(Collider other)
    {
        if (!_armed) return;
        StatueBase sb = other.GetComponent<StatueBase>();
        if (sb == null) return;
        triggerTrap(sb);
    }

    private void triggerTrap(StatueBase sb)
    {
        Debug.Assert(_armed);
        if (_statueLockPosition == null) _statueLockPosition = transform;
        Disarm();
        TransformLerper lerper = GetComponent<TransformLerper>();
        sb.Lock();
        sb.LockMovement();
        _lockedStatue = sb;
        lerper.LerpTo(sb.gameObject, _statueLockPosition, 1f);
        TrapTriggered?.Invoke();
    }

    public void Arm()
    {
        _armed = true;
    }

    public void Disarm()
    {
        _armed = false;
    }

    public delegate void TrapTriggerHandler();
}