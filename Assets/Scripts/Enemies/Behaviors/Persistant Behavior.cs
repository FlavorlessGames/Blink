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

    protected override List<Vector3> targetsInRange()
    {
        List<Vector3> targets = new List<Vector3>();
        foreach (Vector3 position in EntityManager.Instance.GetPlayerPositions())
        {
            targets.Add(position);
        }
        return targets;
    }
}