using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private Vector2 _vector2ZQSD;
    private Vector3 _vector3ZQSD;

    [SerializeField] private Camera _camera;
    [SerializeField] private float speedCam;

    private Trackmania _trackmania;
    private void Awake()
    {
        _trackmania = new Trackmania();
    }

    void Start()
    {
        _trackmania.Player.Move.performed += MoveZQSD;
        _trackmania.Player.Move.canceled += MoveZQSD;
    }

    private void Update()
    {
        _camera.transform.position += _vector3ZQSD * speedCam * Time.deltaTime;
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
}
