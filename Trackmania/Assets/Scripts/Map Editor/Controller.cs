using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Controller : MonoBehaviour
{
    private Vector2 _vector2ZQSD;
    private Vector3 _vector3ZQSD;

    private Vector2 _vector2Zoom;
    private Vector3 _vector3Zoom;

    private Vector2 _vector2Rotation;
    private bool _rotation = false;

    [SerializeField] private Camera _camera;
    [SerializeField] private float speedCam = 15f;
    [SerializeField] private float speedZoom = 0.1f;
    [SerializeField] private float speedRota = 0.1f;

    private Trackmania _trackmania;
    private void Awake()
    {
        _trackmania = new Trackmania();
    }

    void Start()
    {
        _trackmania.Player.Move.performed += MoveZQSD;
        _trackmania.Player.Move.canceled += MoveZQSD;

        _trackmania.Player.Zoom.performed += Zoom;
        _trackmania.Player.Zoom.canceled += Zoom;

        _trackmania.Player.Rotation.performed += InputRotation;
        _trackmania.Player.Rotation.canceled += InputRotation;
    }

    private void Update()
    {
        _camera.transform.position += _vector3ZQSD * speedCam * Time.deltaTime;
        _camera.transform.position += _vector3Zoom * speedZoom * Time.deltaTime;

        if (_rotation)
        {
            Rotation();
        }
    }

    private void OnEnable()
    {
        _trackmania.Player.Enable();
    }

    private void OnDisable()
    {
        _trackmania.Player.Disable();
    }
    public void MoveZQSD(InputAction.CallbackContext context)
    {
        _vector2ZQSD = context.ReadValue<Vector2>();
        _vector3ZQSD = new Vector3(_vector2ZQSD.x, 0, _vector2ZQSD.y);
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        _vector2Zoom = context.ReadValue<Vector2>();
        _vector3Zoom = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z)*-(_vector2Zoom.y);
    }

    public void InputRotation(InputAction.CallbackContext context)
    {
        if (!_rotation)
        {
            _rotation = true;
        }
        else _rotation = false;
    }

    private void Rotation()
    {
        print(Mouse.current.position.ReadValue());
        //_vector2Rotation.x += Mouse.current.position.ReadValue().x * 0.1f;
        //_vector2Rotation.y += Mouse.current.position.ReadValue().y * 0.1f;
        _camera.transform.rotation = Quaternion.Euler(-_vector2Rotation.y * speedRota * Time.deltaTime, _vector2Rotation.x * speedRota * Time.deltaTime, 0);
    }
}
