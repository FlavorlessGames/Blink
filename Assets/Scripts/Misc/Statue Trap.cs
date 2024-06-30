using UnityEngine;

public class StatueTrap : MonoBehaviour
{
    [SerializeField] private bool _armed;
    public event TrapTriggerHandler TrapTriggered;

    void OnTriggerEnter(Collider other)
    {
        StatueBase sb = other.GetComponent<StatueBase>();
        if (sb == null) return;
        triggerTrap(sb);
    }

    private void triggerTrap(StatueBase sb)
    {
        if (!_armed) return;
        sb.Lock();
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