using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StatueBase : MonoBehaviour
{
    [SerializeField] private float _detectionDistance = 100f;
    // [SerializeField] private bool _stopped = false;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _agent;
    private StatueBehavior _statueBehavior;
    public float DetectionRange { get { return _detectionDistance; } }
    private bool _locked = false;

    // Start is called before the first frame update
    void Start()
    {
        _statueBehavior = GetComponent<StatueBehavior>();
        LightDetection lightDetection = GetComponent<LightDetection>();
        if (lightDetection == null) return;
        lightDetection.SpottedEvent += Stop;
        lightDetection.EyesAvertedEvent += Resume;
    }

    public void Stop()
    {
        _agent.velocity = Vector3.zero;
        _agent.ResetPath();
        _agent.isStopped = true;
        // _agent.Sleep();
    }

    public void Resume()
    {
        if (_locked) return;
        _agent.isStopped = false;
    }

    public void Lock()
    {
        _locked = true;
        Stop();
    }

    public void Unlock()
    {
        _locked = false;
    }

    public void SetDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }
}
