using Unity.Netcode;
using UnityEngine;

public class CustomNetworkTransform : NetworkBehaviour
{
    private NetworkVariable<double> _x = new NetworkVariable<double>();
    private NetworkVariable<double> _z = new NetworkVariable<double>();
    private bool _connected = false;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _agent;

    void Start() 
    {
        setValues();
    }

    public override void OnNetworkSpawn()
    {
        _connected = true;
    }

    void FixedUpdate()
    {
        if (!_connected) return;
        Sync();
        setValues();
    }

    public void Sync()
    {
        if (_agent.isStopped) return;
        if (IsServer) return;
        Vector3 currentPos = transform.position;
        currentPos.x = (float) _x.Value;
        currentPos.z = (float) _z.Value;
        transform.position = currentPos;
    }

    private void setValues()
    {
        if (!IsServer) return;
        _x.Value = transform.position.x;
        _z.Value = transform.position.z;
    }
}