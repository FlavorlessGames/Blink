using UnityEngine;

public class DormantBehavior : StatueBehavior
{
    private Vector3 _station;

    protected override void Start()
    {
        _mode = EnemyMode.Dormant;
        base.Start();
    }

    protected override void modeBranch()
    {
        switch (_mode)
        {
            case EnemyMode.Dormant:
                break;
            case EnemyMode.Idle:
                pursueIfInRange();
                break;
            case EnemyMode.Pursuing:
                pursueTarget();
                idleIfOutOfRange();
                break;
            default:
                Debug.LogError(string.Format("Stationary Behavior should not reach mode: {0}", _mode));
                break;
        }
    }
    
    public void Awaken()
    {
        if (_mode != EnemyMode.Dormant) return;
        _mode = EnemyMode.Idle;
    }
}