using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car : MonoBehaviour
{
    [Header("Tire Position")]
    [SerializeField] private Transform _tireFrontL;
    [SerializeField] private Transform _tireFrontR;
    [SerializeField] private Transform _tireBackL;
    [SerializeField] private Transform _tireBackR;

    [Header("Vehicle Stats")]
    [SerializeField] private int _speed = 10;

    private PlayerMap _playerMap;
    private Rigidbody _rigidbody;
    private Vector2 _playerInput;

    private void Awake()
    {
        _playerMap = new PlayerMap();     
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _playerMap.PlayerMovement.Movement.performed += MoveAction;
        _playerMap.PlayerMovement.Movement.canceled += MoveAction;
    }

    private void OnEnable()
    {
        _playerMap.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        _playerMap.PlayerMovement.Disable();
    }

    void Update()
    {
        _rigidbody.AddForceAtPosition(_tireFrontL.forward * _playerInput.y * _speed, _tireFrontL.position);
        _rigidbody.AddForceAtPosition(_tireFrontR.forward * _playerInput.y * _speed, _tireFrontR.position);

        _tireFrontR.localEulerAngles = Vector3.up * (Mathf.Clamp((_playerInput.x * 100), -45, 45) + 180);
        _tireFrontL.localEulerAngles = Vector3.up * (Mathf.Clamp((_playerInput.x * 100), -45, 45) + 180);
    }

    public void MoveAction(InputAction.CallbackContext context)
    {
        _playerInput = context.ReadValue<Vector2>();
    }
}
