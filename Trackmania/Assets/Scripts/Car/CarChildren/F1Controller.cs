using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class F1Controller : MonoBehaviour
{
    [Header("Car stats")]
    [SerializeField] private Transform _centerOfMass;
    [SerializeField] private Transform _frictionPoint;
    [SerializeField] private float _speed = 2000f;
    [SerializeField] private float _turn = 3000f;
    [SerializeField] private float _friction = 6000f;

    [Space(10)]
    [Header("Ground Check")]
    [SerializeField] private Transform _groundRayPoint;
    [SerializeField] private float _maxRayLenght = 0.25f;

    [Space(10)]
    [Header("Threshold")]
    [SerializeField] private float _inputThreshold = 0.1f;
    [SerializeField] private float _carVelocityThreshold = 0.1f;

    private Rigidbody _rigidbody;
    private PlayerMap _playerMap;

    private float _speedValue;
    private float _fricValue;
    private float _turnValue;
    private float _curveVelocity;
    private float _frictionAngle;

    private Vector3 _carVelocity;

    #region Input Variable

    private float _speedInput;
    private float _turnInput;

    #endregion

    private void Awake()
    {
        _playerMap = new PlayerMap();
        _rigidbody = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        _rigidbody.centerOfMass = _centerOfMass.localPosition;

        _playerMap.PlayerMovement.ForwardBackward.performed += ForwardBackward;
        _playerMap.PlayerMovement.ForwardBackward.canceled += ForwardBackward;

        _playerMap.PlayerMovement.LeftRigth.performed += LeftRigth;
        _playerMap.PlayerMovement.LeftRigth.canceled += LeftRigth;
    }

    private void OnEnable()
    {
        _playerMap.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        _playerMap.PlayerMovement.Disable();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Physics.Raycast(_groundRayPoint.position, -transform.up, out hit, _maxRayLenght);

        _carVelocity = transform.InverseTransformDirection(_rigidbody.velocity);

        if(hit.collider)
        {
            AccelerationLogic();
            TurningLogic();
            FrictionLogic();
            Debug.DrawLine(_groundRayPoint.position, hit.point, Color.green);

            _rigidbody.centerOfMass = Vector3.zero;
        }
    }

    private void AccelerationLogic()
    {
        _speedValue = _speed * _speedInput * Time.fixedDeltaTime * 100;

        if(_speedInput > _inputThreshold)
        {
            _rigidbody.AddForceAtPosition(transform.forward * _speedValue, _groundRayPoint.position);
        }

        else if(_speedInput < -_inputThreshold)
        {
            _rigidbody.AddForceAtPosition(transform.forward * _speedValue/2, _groundRayPoint.position);
        }
    }

    private void TurningLogic()
    {
        _turnValue = _turn * _turnInput * Time.fixedDeltaTime * 100;

        if(Mathf.Abs(_carVelocity.z) > _carVelocityThreshold)
        {
            _rigidbody.AddTorque(transform.up * _turnValue);
        }
    }

    private void FrictionLogic()
    {
        _fricValue = _friction;

        if(_carVelocity.magnitude > 1)
        {
            _frictionAngle = (-Vector3.Angle(transform.up, Vector3.up) / 90f) + 1;
            _rigidbody.AddForceAtPosition(transform.right * _fricValue * _frictionAngle * 100 * -_carVelocity.normalized.x, _frictionPoint.position);
        }
    }

    private void ForwardBackward(InputAction.CallbackContext context)
    {
        _speedInput = context.ReadValue<float>();
    }

    private void LeftRigth(InputAction.CallbackContext context)
    {
        _turnInput = context.ReadValue<float>();
    }
}
