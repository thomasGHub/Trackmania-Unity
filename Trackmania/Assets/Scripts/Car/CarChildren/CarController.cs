using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : Car
{
    [Header("Tire Script")]
    [SerializeField] private Wheel[] _allTires;

    [Space(10)]
    [Header("Tire Position")]
    [SerializeField] private Transform _frontLeftTire;
    [SerializeField] private Transform _rearRigthTire;

    [Header("Vehicle Stats")]
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _whellBase; //in UnityUnits distance between frontTire and rearTire
    [SerializeField] private float _rearTrack; //in UnityUnits distance between leftTire and RigthTire
    [SerializeField] private float _turnRadius = 10; //in UnityUnits point around the car will rotate

    private PlayerMap _playerMap;

    public float Speed => _speed;

    private float numberOfTire;

    private void Awake()
    {
        _playerMap = new PlayerMap();
    }

    void Start()
    {
        numberOfTire = _allTires.Length;

        _whellBase = Mathf.Abs(_frontLeftTire.transform.position.x - _rearRigthTire.transform.position.x);
        _rearTrack = Mathf.Abs(_frontLeftTire.transform.position.z - _rearRigthTire.transform.position.z);

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

    public void ForwardBackward(InputAction.CallbackContext context)
    {
        float playerInput = context.ReadValue<float>();

        for (int i = 0; i < numberOfTire; i++)
            if(_allTires[i].IsDrivingWheel)
                _allTires[i].PlayerInputFB = playerInput;

    }

    public void LeftRigth(InputAction.CallbackContext context)
    {
        float playerInput = context.ReadValue<float>();
        float ackermannAngleLeft;
        float ackermannAngleRight;

        // Use ackermann formula

        if(playerInput > 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(_whellBase/ (_turnRadius + (_rearTrack/2))) * playerInput; //inter Wheel
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(_whellBase/ (_turnRadius - (_rearTrack/2))) * playerInput; //extern Wheel
        }
        else if(playerInput < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(_whellBase / (_turnRadius - (_rearTrack / 2))) * playerInput; //extern Wheel
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(_whellBase / (_turnRadius + (_rearTrack / 2))) * playerInput; //inter Wheel
        }
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        for (int i = 0; i < numberOfTire; i++)
            if (_allTires[i].CanRotate)
            {
                if (_allTires[i].IsLeft)
                    _allTires[i].SteerAngle = ackermannAngleLeft;
                else
                    _allTires[i].SteerAngle = ackermannAngleRight;
            }
    }
}
