using UnityEngine;

public class Firefly : MonoBehaviour
{
    [SerializeField] private float _tickRate = 0.3f;
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody _rb;
    public Vector3 Velocity { get { return _velocity; } }
    private float _tickTimer = 0f;
    private Swarm _swarm;
    private Vector3 _direction;
    private Vector3 _velocity;
    private const float c_ProtectedRange = .5f;
    private Vector3 _averagePosition;
    private Vector3 _closeDiff;
    private Vector3 _averageVelocity;
    private int _flockCount;

    void Start()
    {
        _swarm = transform.parent.GetComponent<Swarm>();
        if (_swarm == null) Debug.LogError("Swarm not found");
        _swarm.AddFirefly(this);
    }

    void Update()
    {
        resetAccumulatorValues();
        computeDifferences();
        turnToCenter();
        matchVelocities();
        avoidCrashing();
        capVelocity();
        updatePosition();
    }

    void resetAccumulatorValues()
    {
        _averagePosition = Vector3.zero;
        _averageVelocity = Vector3.zero;
        _closeDiff = Vector3.zero;
        _flockCount = 0;
    }

    void computeDifferences()
    {
        foreach(Firefly ff in _swarm.Flock)
        {
            if (ff == this) continue;

            float distance = getDistance(ff);
            
            if (distance < c_ProtectedRange)
            {
                _closeDiff += transform.position - ff.transform.position;
            }

            _averagePosition += ff.transform.position;
            _averageVelocity += ff.Velocity;

            _flockCount++;
        }

        _averagePosition /= _flockCount;
        _averageVelocity /= _flockCount;
    }

    void turnToCenter()
    {
        _velocity = _rb.velocity + (_averagePosition - transform.position) * _swarm.CenteringFactor;
    }

    void avoidCrashing()
    {
        _velocity += _rb.velocity + _closeDiff * _swarm.AvoidFactor;
    }

    void matchVelocities()
    {
        _velocity += _rb.velocity + (_averageVelocity - Velocity) * _swarm.MatchingFactor;
    }

    void updatePosition()
    {
        _rb.position = _rb.position + _velocity * Time.deltaTime;
    }

    void capVelocity()
    {
        float velocity = _velocity.magnitude;
        if (velocity > 111)
        {
            _velocity = _velocity / velocity * 111;
        }
        if (velocity < 5)
        {
            _velocity = _velocity / velocity * 5;
        }
    }
    float getDistance(Firefly firefly)
    {
        return Vector3.Distance(transform.position, firefly.transform.position);
    }
}