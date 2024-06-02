using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _walkSpeed = 5.0f;
    [SerializeField] private GameObject _target;
    [SerializeField] private UnityEngine.AI.NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(_target.transform.position);
        // move();
    }

    private void move()
    {
        Vector3 direction = getDirection();
        _controller.Move(direction * _walkSpeed * Time.deltaTime);
    }

    private Vector3 getDirection()
    {
        Vector3 direction = new Vector3(1,0,0);
        return direction;
    }
}