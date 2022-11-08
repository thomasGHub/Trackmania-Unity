using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tire : MonoBehaviour
{
    [Header("Attribute needed")]
    [SerializeField] private Rigidbody _carRigidbody;
    [SerializeField] private CarController _car;

    [Space(10)]
    [Header("Tire stats")]
    [SerializeField] private bool _canRotate;
    [SerializeField] private bool _isFront;
    [SerializeField] private bool _isLeft;
    [SerializeField] private float _steerTime = 8f;

    [System.NonSerialized] public float _playerInputFB = 0; // Forward / Backward
    [System.NonSerialized] public float _playerInputLR = 0; // Left / Rigth

    [System.NonSerialized] public float SteerAngle = 0f;

    public bool IsFront => _isFront;
    public bool IsLeft => _isLeft;

    private float _whellRadius;
    private float _whellAngle = 0;
    
    private void Start()
    {
        _whellRadius = GetComponent<MeshRenderer>().bounds.extents.y;
    }

    private void Update()
    {
        _whellAngle = Mathf.Lerp(_whellAngle, SteerAngle, _steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * _whellAngle);
    }

    private void FixedUpdate()
    {
        
    }


}
