using UnityEngine;

public class StatueTrap : MonoBehaviour
{
    [SerializeField] private bool _armed;
    public event TrapTriggerHandler TrapTriggered;
    private List<StatueBase> _lockedStatues;

    void Start()
    {
        _lockedStatues = new List<StatueBase>();
    }

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
        _lockedStatues.Add(sb);
        TrapTriggered?.Invoke();
    }

    public void FreeStatues()
    {
        foreach (StatueBase sb in _lockedStatues)
        {
            sb.Unlock();
        }

        _lockedStatues.Clear();
    }

    public void Arm()
    {
        _armed = true;
    }

    public void Disarm()
    {
        _armed = false;
        FreeStatues();
    }

    public delegate void TrapTriggerHandler();
}