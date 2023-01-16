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

    [SerializeField] private Camera _camera;
    [SerializeField] private float speedCam = 15f;
    [SerializeField] private float speedZoom = 0.1f;
    [SerializeField] private float speedRota = 0.3f;

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
        Vector3 _moveX = new Vector3(_camera.transform.right.x, 0, _camera.transform.right.z);
        Vector3 _moveZ = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);
        _camera.transform.position += (_vector3ZQSD.x * _moveX + _vector3ZQSD.z * _moveZ) * speedCam * Time.deltaTime;
        _camera.transform.position += _vector3Zoom * speedZoom * Time.deltaTime;
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
        //_vector3ZQSD = new Vector3(_camera.transform.forward.x, 0, _camera.transform.forward.z);

    }

    public void Zoom(InputAction.CallbackContext context)
    {
        _vector2Zoom = context.ReadValue<Vector2>();
        _vector3Zoom = new Vector3(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z) * -(_vector2Zoom.y);
    }

    public void InputRotation(InputAction.CallbackContext context)
    {
        if (!Mouse.current.rightButton.isPressed)
            return;

        float valueX = context.ReadValue<Vector2>().x;
        float valueY = context.ReadValue<Vector2>().y;
        _camera.transform.rotation = Quaternion.Euler(-valueY * speedRota + _camera.transform.rotation.eulerAngles.x, valueX * speedRota + _camera.transform.rotation.eulerAngles.y, 0f);
    }
}
