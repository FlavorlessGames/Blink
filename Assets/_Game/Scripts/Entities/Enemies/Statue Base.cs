using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using fgames.Debug;

public class StatueBase : MonoBehaviour
{
    [Range(1f, 100f)]
    [SerializeField] private float _detectionDistance = 100f;
    // [SerializeField] private bool _stopped = false;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _agent;
    [SerializeField] private List<Renderer> _visuals = new ();
    private StatueBehavior _statueBehavior;
    public float DetectionRange { get { return _detectionDistance; } }
    private bool _locked = false;
    private bool _stopped = false;
    public bool Stopped { get { return getStopped(); } }

    // Start is called before the first frame update
    void Start()
    {
        _statueBehavior = GetComponent<StatueBehavior>();
        LightDetection lightDetection = GetComponent<LightDetection>();
        if (lightDetection == null) return;
        lightDetection.SpottedEvent += Stop;
        lightDetection.EyesAvertedEvent += Resume;
        if (DebugManager.Instance.DisableStatues) Lock();
        DebugManager.Instance.DisableStatuesUpdate += updateStatuesEnabled;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionDistance);
    }

    void Update()
    {
        _agent.isStopped = getStopped();
    }

    private bool getStopped()
    {
        return _stopped || _locked || DebugManager.Instance.DisableStatues;
    }

    public void Stop()
    {
        setVisibility(true);
        if (!_agent.enabled) return;
        _agent.velocity = Vector3.zero;
        _agent.ResetPath();
        // _agent.isStopped = true;
        _stopped = true;
        // _agent.Sleep();
    }

    public void Resume()
    {
        if (_locked) return;
        if (!_agent.enabled) return;
        setVisibility(false);
        // _agent.isStopped = false;
        _stopped = false;
    }

    public void Lock()
    {
        Debug.Log("Lock");
        _locked = true;
        Stop();
    }

    public void Unlock()
    {
        _locked = false;
        Resume();
    }

    public void LockMovement()
    {
        _agent.enabled = false;
    }
    
    private void setVisibility(bool canSee)
    {
        foreach (Renderer r in _visuals)
        {
            r.enabled = canSee;
        }
        // Renderer renderer = GetComponent<Renderer>();
        // renderer.enabled = canSee;
    }

    public void SetDestination(Vector3 destination)
    {
        // Debug.Log(destination);
        if (!_agent.enabled) return;
        _agent.SetDestination(destination);
    }

    private void updateStatuesEnabled(bool flag)
    {
        if (flag)
        {
            Lock();
            return;
        }
        Unlock();
    }
}
