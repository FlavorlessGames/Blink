using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StatueMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _height = 1.5f;
    [SerializeField] private float _fallSpeed = 3f;
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

    private void fall()
    {
        if (grounded()) return;
        _controller.Move(-transform.up * _fallSpeed * Time.deltaTime);
    }

    private bool grounded()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return transform.position.y - hit.point.y < _height;
        }
        return true;
    }

    private void move(Vector3 target)
    {
        _agent.SetDestination(target);
    }

    // private Vector3 getDirection()
    // {
    //     Vector3 direction = new Vector3(0,0,0);
    //     Vector3 target = selectTarget();
    //     if (target == Vector3.zero) return direction;
    //     Vector3 diff = target - transform.position;
    //     spawnIndicator(transform.position + diff/2);
    //     direction = diff.normalized;
    //     return direction;
    // }

    private Vector3 selectTarget()
    {
        Vector3 target = new Vector3(0,0,0);
        if (EntityManager.Instance == null) return target;
        List<Vector3> possibleTargets = new List<Vector3>();
        Dictionary<Vector3, Path> targetPaths = new Dictionary<Vector3, Path>();
        foreach (Vector3 position in EntityManager.Instance.GetPlayerPositions())
        {
            if (!Utility.InRange(transform.position, position, _detectionDistance)) continue;
            Path path = getPath(position);
            targetPaths[position] = path;
            if (!path.Valid) continue;
            possibleTargets.Add(position);
        }
        foreach (Vector3 position in possibleTargets)
        {
            target = closest(transform.position, target, position);
        }
        return target;
    }

    private Path getPath(Vector3 position)
    {
        Path path = new Path();
        path.Valid = canSeePlayer(transform.position, position);
        return path;
    }

    private Path getPathRecursive(int level, Path basePath, Vector3 position)
    {
        return basePath;
    }

    private Vector3 closest(Vector3 start, Vector3 pos1, Vector3 pos2)
    {
        if (pos1 == Vector3.zero) return pos2;
        if (pos2 == Vector3.zero) return pos1;
        var distance1 = Vector3.Distance(start, pos1);
        var distance2 = Vector3.Distance(start, pos2);
        return distance1 < distance2 ? pos1 : pos2;
    }

    // private bool inRange(Vector3 position)
    // {
    //     return Vector3.Distance(transform.position, position) <= _detectionDistance;
    // }

    private void spawnIndicator(Vector3 position)
    {
        GameObject indicator = Instantiate(_pathIndicator, position, new Quaternion(0,0,0,0));
        indicator.transform.parent = this.transform;
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

    private bool canSeePlayer(Vector3 from, Vector3 playerPosition)
    {
        Vector3 direction = (playerPosition - from).normalized;
        Ray ray = new Ray(from, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _detectionDistance))
        {
            PlayerAccess player = hit.transform.GetComponent<PlayerAccess>();
            return player != null;
        }
        return false;
    }

    private struct Path
    {
        public bool Valid;
        public Vector3 Next;
        public float Distance;
    }
}
