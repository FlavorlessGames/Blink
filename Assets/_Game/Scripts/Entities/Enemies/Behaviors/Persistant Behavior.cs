using UnityEngine;
using System.Collections.Generic;

public class PersistantBehavior : StatueBehavior
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
            case EnemyMode.Pursuing:
                pursueTarget();
                break;
            default:
                Debug.LogError(string.Format("Persistant Behavior should not reach mode: {0}", _mode));
                break;
        }
    }
}