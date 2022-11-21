using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Wheel : MonoBehaviour
{
    [Header("Attribute needed")]
    [SerializeField] private Rigidbody _carRigidbody;
    [SerializeField] private CarController _car;
    [SerializeField] private GameObject _wheelModel;

    [Space(10)]
    [Header("Wheel")]
    [SerializeField] private bool _drivingWheel;
    [SerializeField] private bool _canRotate;
    [SerializeField] private bool _isFront;
    [SerializeField] private bool _isLeft;
    [SerializeField] private float _steerTime = 8f;

    [Space(10)]
    [Header("Suspension")]
    [SerializeField] private float _springTravel;
    [SerializeField] private float _springStiffness;
    [SerializeField] private float _damperStiffness;

    private float _restLength;
    private float _springLength;
    private float _minLength;
    private float _maxLength;
    private float _lastLength;
    private float _springForce;
    private float _damperForce;
    private Vector3 _suspensionForce;
    private float _springVelocity;

    private float _wheelRadius;
    private float _wheelAngle = 0;
    private Vector3 _whellOffSetRotation;

    private Vector3 _wheelVelocityLS; // Local space velociy
    private float _engineForce;
    private float _gripForce;

    [System.NonSerialized] public float PlayerInputFB = 0; // Forward / Backward
    [System.NonSerialized] public float SteerAngle = 0f;

    public bool IsFront => _isFront;
    public bool IsLeft => _isLeft;
    public bool IsDrivingWheel => _drivingWheel;
    public bool CanRotate => _canRotate;

    private void Start()
    {
        _wheelRadius = _wheelModel.GetComponent<MeshRenderer>().bounds.extents.y;
        _restLength = transform.position.y - _wheelModel.transform.position.y;

        _minLength = _restLength - _springTravel;
        _maxLength = _restLength + _springTravel;

        _whellOffSetRotation = _wheelModel.transform.localRotation.eulerAngles;
    }

    private void Update()
    {
        _wheelAngle = Mathf.Lerp(_wheelAngle, SteerAngle, _steerTime * Time.deltaTime);
        _wheelModel.transform.localRotation = Quaternion.Euler(_wheelModel.transform.localRotation.x + _whellOffSetRotation.x, _wheelModel.transform.localRotation.y + _wheelAngle + _whellOffSetRotation.y, _wheelModel.transform.localRotation.z + _whellOffSetRotation.z);
        //transform.localRotation = Quaternion.Euler(Vector3.up * _wheelAngle);
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit,_maxLength + _wheelRadius))
        {
            #region Calc Suspension Force
            _lastLength = _springLength;
            _springLength = hit.distance - _wheelRadius;
            _springLength = Mathf.Clamp(_springLength, _minLength, _maxLength);
            _springVelocity = (_lastLength - _springLength) / Time.fixedDeltaTime;

            _springForce = _springStiffness * (_restLength - _springLength);
            _damperForce = _damperStiffness * _springVelocity;

            _suspensionForce = (_springForce + _damperForce) * transform.up;

            #endregion

            _wheelVelocityLS = transform.InverseTransformDirection(_carRigidbody.GetPointVelocity(hit.point));

            _engineForce = PlayerInputFB * 10000;
            _gripForce = _wheelVelocityLS.x * 1000;

            _carRigidbody.AddForceAtPosition(_suspensionForce + (transform.forward * _engineForce) + (_gripForce * transform.right), hit.point);
        }

        DrawArrow.ForDebug(transform.position, _suspensionForce, Color.black);
        DrawArrow.ForDebug(transform.position, _gripForce * transform.right, Color.green);
        DrawArrow.ForDebug(transform.position, _engineForce * transform.forward, Color.blue);
    }


}
