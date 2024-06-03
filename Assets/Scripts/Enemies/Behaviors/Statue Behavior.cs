using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class StatueBehavior : MonoBehaviour
{
    [SerializeField] protected EnemyMode _mode;
    protected StatueBase _base;

    protected virtual void Start()
    {
        _base = GetComponent<StatueBase>();
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
        if (targetsInRange().Count <= 0) return;
        _mode = EnemyMode.Pursuing;
        _base.Resume();
    }

    protected virtual void pursueTarget()
    {
        Vector3 target = selectTarget();
        if (target == Vector3.zero) return;
        _base.SetDestination(target);
    }

    protected void idleIfOutOfRange()
    {
        if (targetsInRange().Count > 0) return;
        _mode = EnemyMode.Idle;
        _base.Stop();
    }

    protected virtual List<Vector3> targetsInRange()
    {
        List<Vector3> targets = new List<Vector3>();
        foreach (Vector3 position in EntityManager.Instance.GetPlayerPositions())
        {
            if (!Utility.InRange(transform.position, position, _base.DetectionRange)) continue;
            targets.Add(position);
        }
        return targets;
    }

    protected Vector3 selectTarget()
    {
        Vector3 target = new Vector3(0,0,0);
        if (EntityManager.Instance == null) return target;
        foreach (Vector3 position in targetsInRange())
        {
            target = closest(transform.position, target, position);
        }
        return target;
    }

    protected Vector3 closest(Vector3 start, Vector3 pos1, Vector3 pos2)
    {
        if (pos1 == Vector3.zero) return pos2;
        if (pos2 == Vector3.zero) return pos1;
        var distance1 = Vector3.Distance(start, pos1);
        var distance2 = Vector3.Distance(start, pos2);
        return distance1 < distance2 ? pos1 : pos2;
    }

    // [Rpc(SendTo.Everyone)]
    // private void setDestinationRpc(Vector3 destination)
    // {
    //     _base.SetDestination(destination);
    // }
}
