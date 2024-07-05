using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

[RequireComponent(typeof(StatueBase))]
[RequireComponent(typeof(EnemyAccess))]
public class StatueBehavior : MonoBehaviour
{
    [SerializeField] protected EnemyMode _mode;
    protected StatueBase _base;
    protected EnemyAccess _ea;

    protected virtual void Start()
    {
        _base = GetComponent<StatueBase>();
        _ea = GetComponent<EnemyAccess>();
        if (_base == null) Debug.LogError("Statue Base not assigned");
    }

    void FixedUpdate()
    {
        modeBranch();
    }

    // public override void OnNetworkSpawn() 
    // {
    //     if (!IsServer) enabled = false;
    // }

    protected virtual void modeBranch()
    {
        Debug.Log("Base Mode Branch Method. Should not be called");
    }

    protected void pursueIfInRange()
    {
        if (targetInRange()) return;
        _mode = EnemyMode.Pursuing;
        _base.Resume();
    }

    protected virtual void pursueTarget()
    {
        PlayerAccess target = selectTarget();
        EntityManager.Instance.RegisterTarget(_ea, target);
        Vector3 destination = EntityManager.Instance.GetPath(_ea);
        if (destination == Vector3.zero) return;
        _base.SetDestination(destination);
    }

    protected void idleIfOutOfRange()
    {
        if (!targetInRange()) return;
        EntityManager.Instance.ClearTarget(_ea);
        _mode = EnemyMode.Idle;
        _base.Stop();
    }

    protected bool targetInRange()
    {
        foreach (PlayerAccess pa in EntityManager.Instance.GetPlayers())
        {
            if (!Utility.InRange(transform.position, pa.Position, _base.DetectionRange)) return true;
        }
        return false;
    }

    protected virtual List<PlayerAccess> validTargets()
    {
        List<PlayerAccess> targets = new List<PlayerAccess>();
        foreach (PlayerAccess pa in EntityManager.Instance.GetPlayers())
        {
            if (!Utility.InRange(transform.position, pa.Position, _base.DetectionRange)) continue;
            targets.Add(pa);
        }
        return targets;
    }

    protected PlayerAccess selectTarget()
    {
        PlayerAccess target = null;
        foreach (PlayerAccess pa in validTargets())
        {
            if (target == null) target = pa;
            if (closest(transform.position, target.Position, pa.Position))
            {
                target = pa;
            }
        }
        return target;
    }

    protected bool closest(Vector3 start, Vector3 pos1, Vector3 pos2)
    {
        if (pos1 == Vector3.zero) return false;
        if (pos2 == Vector3.zero) return true;
        var distance1 = Vector3.Distance(start, pos1);
        var distance2 = Vector3.Distance(start, pos2);
        return distance1 < distance2; 
    }

    // [Rpc(SendTo.Everyone)]
    // private void setDestinationRpc(Vector3 destination)
    // {
    //     _base.SetDestination(destination);
    // }
}
