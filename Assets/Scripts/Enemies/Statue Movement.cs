using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StatueMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _detectionDistance = 100f;
    [SerializeField] private bool _stopped = false;
    [SerializeField] private GameObject _pathIndicator;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _agent;
    private const int _pathingLevels = 2; // Warning: Do not modify

    // Start is called before the first frame update
    void Start()
    {
        LightDetection lightDetection = GetComponent<LightDetection>();
        if (lightDetection == null) return;
        lightDetection.SpottedEvent += stop;
        lightDetection.EyesAvertedEvent += resume;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stopped) return;
        Vector3 target = selectTarget();
        if (target == Vector3.zero) return;
        move(target);
    }

    private void stop()
    {
        _agent.isStopped = true;
        _stopped = true;
    }

    private void resume()
    {
        _agent.isStopped = false;
        _stopped = false;
    }

    private void move(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    private Vector3 selectTarget()
    {
        Vector3 target = new Vector3(0,0,0);
        if (EntityManager.Instance == null) return target;
        List<Vector3> possibleTargets = new List<Vector3>();
        foreach (Vector3 position in EntityManager.Instance.GetPlayerPositions())
        {
            if (!Utility.InRange(transform.position, position, _detectionDistance)) continue;
            possibleTargets.Add(position);
        }
        foreach (Vector3 position in possibleTargets)
        {
            target = closest(transform.position, target, position);
        }
        return target;
    }

    private Vector3 closest(Vector3 start, Vector3 pos1, Vector3 pos2)
    {
        if (pos1 == Vector3.zero) return pos2;
        if (pos2 == Vector3.zero) return pos1;
        var distance1 = Vector3.Distance(start, pos1);
        var distance2 = Vector3.Distance(start, pos2);
        return distance1 < distance2 ? pos1 : pos2;
    }
}
