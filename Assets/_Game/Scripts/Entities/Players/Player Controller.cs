using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _walkSpeed = 5.0f;
    [SerializeField] private float _runSpeed = 7.5f;
    [SerializeField] private float _lookSpeed = 5.0f;
    [SerializeField] private float _lookLimitX = 100.0f;
    [SerializeField] private float _height = 1.5f;
    [SerializeField] private float _fallSpeed = 3f;
    [SerializeField] private bool _stopped = false;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _pov;
    [SerializeField] private float _jumpPower = 8f;
    private float _yVelocity;
    private float _currentSpeed;
    private Camera _cam;
    private float _rotationX = 0f;
    private bool _disabled = false;
    public bool Running { get {
        Vector3 direction = getMovementDirection();
        return _currentSpeed == _runSpeed && (
            direction.magnitude > 0f
        );
    }}

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _walkSpeed;
        _cam = _camera.GetComponent<Camera>();
    }

    public override void OnNetworkSpawn() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        _camera.SetActive(IsOwner);
        if (!IsOwner) _disabled=true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_disabled) return;
        fall();
        if (_stopped) return;
        if (PauseManager.Instance.IsPaused) return;
        run();
        move();
        look();
        jump();
    }

    private void fall()
    {
        if (_yVelocity > 0)
        {
            _yVelocity -= (_fallSpeed * Time.deltaTime);
            if (_yVelocity < 0) _yVelocity = 0f;
            _controller.Move(new Vector3(0, _yVelocity, 0) * Time.deltaTime);
        }
        if (grounded()) {
            _yVelocity = 0f;
            return;
        }
        _controller.Move(-transform.up * _fallSpeed * Time.deltaTime);
    }

    private void jump()
    {
        if (!Input.GetButtonDown("Jump")) return;
        if (!grounded()) return;
        // Debug.Log("Jump");
        _yVelocity = _jumpPower;
        // _controller.Move(new Vector3(0, 1500, 0) * Time.deltaTime);
    }

    private void run()
    {
        // if (DebugManager.Instance.DisableSprint) return;
        _currentSpeed = Input.GetButton("Run") ? _runSpeed : _walkSpeed;
        // Debug.Log(_currentSpeed);
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

    private void move()
    {
        Vector3 direction = getMovementDirection();
        if (!(direction.magnitude > 0))
        {
            return;
        }
        Vector3 relativeDirection = transform.TransformDirection(direction);
        _controller.Move(relativeDirection * _currentSpeed * Time.deltaTime);
    }

    private void look()
    {
        if (_stopped)
        {
            return;
        }
        _rotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_lookLimitX, _lookLimitX);
        Quaternion xDirection = Quaternion.Euler(_rotationX, 0, 0);
        Quaternion yDirection = Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
        lookX(xDirection);
        lookY(yDirection);
    }

    private Vector3 getMovementDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        return direction;
    }

    private void lookX( Quaternion direction)
    {
        _pov.transform.localRotation = direction;
    }

    private void lookY(Quaternion direction)
    {
        transform.rotation *= direction;
    }
}
