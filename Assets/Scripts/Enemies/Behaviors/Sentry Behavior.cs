using UnityEngine;

public class SentryBehavior : StatueBehavior
{
    private Vector3 _station;

    protected override void Start()
    {
        _station = transform.position;
        base.Start();
    }

    protected override void modeBranch()
    {
        switch (_mode)
        {
            case EnemyMode.Idle:
                pursueIfInRange();
                break;
            case EnemyMode.Pursuing:
                pursueTarget();
                returnIfOutOfRange();
                break;
            case EnemyMode.Returning:
                returnToStation();
                idleIfAtStation();
                pursueIfInRange();
                break;
            default:
                Debug.LogError(string.Format("Sentry Behavior should not reach mode: {0}", _mode));
                break;
        }
    }

    protected void returnToStation()
    {
        _base.SetDestination(_station);
    }

    protected void returnIfOutOfRange()
    {
        if (targetsInRange().Count > 0) return;
        _mode = EnemyMode.Returning;
        _base.Stop();
    }

    protected void idleIfAtStation()
    {
        if (Vector3.Distance(_station, transform.position) > .25f) return;
        _mode = EnemyMode.Idle;
        _base.Stop();
    }
}