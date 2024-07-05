using UnityEngine;

public class StationaryBehavior : StatueBehavior
{
    protected override void modeBranch()
    {
        switch (_mode)
        {
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
}