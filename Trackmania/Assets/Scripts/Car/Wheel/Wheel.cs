using UnityEngine;

namespace Car
{
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

        [Space(10)]
        [Header("WheelData")]
        [SerializeField] private WheelData _wheelData;

        #region WhellData Copy

        private float _steeringTime;

        private float _springTravel;
        private float _springStiffness;
        private float _damperStiffness;

        private float _gripFactor;

        #endregion

        #region Suspension Variable

        private float _restLength;
        private float _springLength;
        private float _minLength;
        private float _maxLength;
        private float _lastLength;
        private float _springForce;
        private float _damperForce;
        private float _suspensionForce;
        private float _springVelocity;

        #endregion

        #region Rotation Wheel Variable

        private float _wheelRadius;
        private float _wheelAngle = 0;
        private Vector3 _whellOffSetRotation;
        [System.NonSerialized] public float SteerAngle = 0f;

        #endregion

        #region Engine Variable

        private float _engineForce;
        [System.NonSerialized] public float EnginePower = 0; // Forward / Backward

        #endregion

        #region Grip Variable

        private Vector3 _wheelVelocityLS; // Local space velociy
        private float _gripForce;

        #endregion

        public bool IsFront => _isFront;
        public bool IsLeft => _isLeft;
        public bool IsDrivingWheel => _drivingWheel;
        public bool CanRotate => _canRotate;

        private void Start()
        {
            //Copy WheelData Value

            _steeringTime = _wheelData.SteeringTime;

            _springTravel = _wheelData.SpringTravel;
            _springStiffness = _wheelData.SpringStiffness;
            _damperStiffness = _wheelData.DamperStiffness;

            _gripFactor = _wheelData.GripFactor;

            //End Copy WheelData Value

            _wheelRadius = _wheelModel.GetComponent<MeshRenderer>().bounds.extents.y;
            _restLength = transform.position.y - _wheelModel.transform.position.y;

            _minLength = _restLength - _springTravel;
            _maxLength = _restLength + _springTravel;

            _whellOffSetRotation = _wheelModel.transform.localRotation.eulerAngles;
        }

        private void Update()
        {

            _wheelAngle = Mathf.Lerp(_wheelAngle, SteerAngle, _steeringTime * Time.deltaTime);
            _wheelModel.transform.localRotation = Quaternion.Euler(_wheelModel.transform.localRotation.x + _whellOffSetRotation.x, _wheelModel.transform.localRotation.y + _wheelAngle + _whellOffSetRotation.y, _wheelModel.transform.localRotation.z + _whellOffSetRotation.z);

        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _maxLength + _wheelRadius))
            {
                #region Calc Suspension Force
                _lastLength = _springLength;
                _springLength = hit.distance - _wheelRadius;
                _springLength = Mathf.Clamp(_springLength, _minLength, _maxLength);
                _springVelocity = (_lastLength - _springLength) / Time.fixedDeltaTime;

                _springForce = _springStiffness * (_restLength - _springLength);
                _damperForce = _damperStiffness * _springVelocity;

                _suspensionForce = _springForce + _damperForce;

                #endregion
                Vector3 globalVel = _carRigidbody.GetPointVelocity(hit.point);
                _wheelVelocityLS = transform.InverseTransformDirection(globalVel);

                //_engineForce = PlayerInputFB * _carRigidbody.mass * 5;
                //_gripForce = _wheelVelocityLS.x * _gripFactor * _carRigidbody.mass;

                _engineForce = EnginePower * _springForce * 100;
                _gripForce = _wheelVelocityLS.x * _springForce * 30 * _gripFactor;

                Vector3 totalForce = (_suspensionForce * transform.up) + (_engineForce * transform.forward) + (_gripForce * -transform.right);
                _carRigidbody.AddForceAtPosition(totalForce, hit.point);

                /*DrawArrow.ForDebug(transform.position, _wheelVelocityLS, Color.blue);
                DrawArrow.ForDebug(transform.position, totalForce, Color.red);*/
            }
        }


    }
}

