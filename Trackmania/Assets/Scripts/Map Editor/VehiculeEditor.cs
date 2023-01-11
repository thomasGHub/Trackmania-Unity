using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using MirrorBasics;

    public class VehiculeEditor : MonoBehaviour
    {
        [System.Serializable]
        class Skid
        {
            public TrailRenderer trailRenderer;
            public ParticleSystem particleSystem;
        }


        [Header("Visual")]
        [SerializeField] private Transform[] _allTiresMesh;
        [SerializeField] private Transform[] _rotatingTiresMesh;

        [Space(10)]
        [Tooltip("Factor that reduce the rotation speed of wheel")]
        [SerializeField] private float _rotatingFactor = 3f;
        [SerializeField] private float _turnAngle = 30f;
        [SerializeField] private float _turnDuration = 0.2f;

        [Space(10)]
        [SerializeField] private Skid[] _skids;
        [SerializeField] private float _skidWidth = 0.3f;

        [Header("Car stats")]
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Transform _frictionPoint;
        [SerializeField] private float _speed = 2000f;
        [SerializeField] private float _turn = 3000f;
        [SerializeField] private float _friction = 6000f;
        [SerializeField] private float _dragAmount = 5f;

        [Header("Ground Check")]
        [SerializeField] private Transform _groundRayPoint;
        [SerializeField] private float _maxRayLenght = 0.25f;

        [Header("Threshold")]
        [SerializeField] private float _inputThreshold = 0.1f;
        [SerializeField] private float _carVelocityThreshold = 0.1f;
        [SerializeField] private float _skidThreshold = 20f;


        [Header("Curve")]
        [SerializeField] private AnimationCurve _frictionCurve;
        [SerializeField] private AnimationCurve _speedCurve;
        [SerializeField] private AnimationCurve _turnCurve;
        [SerializeField] private AnimationCurve _driftCurve;
        [SerializeField] private AnimationCurve _engineCurve;

        [Header("Audio")]
        [SerializeField] private AudioSource _engineSoundHighPitched;
        [SerializeField] private AudioSource _engineSoundLowPitched;
        [SerializeField] private AudioSource _skidSound;

        private Rigidbody _rigidbody;
        private PlayerMap _playerMap;

        private float _speedValue;
        private float _fricValue;
        private float _turnValue;
        private float _frictionAngle;

        private bool _grounded;

        private float _scaleValue = 100f; // Value that divide the car Velocity to pass it in 100 Unit per hour
        private float _multiplicatorValue = 1000f; // Value that multiply the speed, turn and friction value

        [SerializeField] private Vector3 _carVelocity;
        private float _carSpeed; // limitate the call "_carVelocity.magnitude"

        private Ghost _ghost;
        private IEnumerator _ghostSaveCoroutine;


        #region Input Variable

        private float _speedInput;
        private float _turnInput;

        #endregion

        public bool isLocalPlayer = true;

        private void Awake()
        {
            _playerMap = new PlayerMap();
            _rigidbody = GetComponent<Rigidbody>();
            _ghost = new Ghost(transform);
        }

        private void Start()
        {


            for (int index = 0; index < _skids.Length; index++)
                _skids[index].trailRenderer.startWidth = _skidWidth;

            if (!isLocalPlayer)
            {
                return;
            }

            _rigidbody.centerOfMass = _centerOfMass.localPosition;

            _playerMap.PlayerMovement.ForwardBackward.performed += ForwardBackward;
            _playerMap.PlayerMovement.ForwardBackward.canceled += ForwardBackward;

            _playerMap.PlayerMovement.LeftRigth.performed += LeftRigth;
            _playerMap.PlayerMovement.LeftRigth.canceled += LeftRigth;
        }

        private void OnDisable()
        {
            _playerMap.PlayerMovement.Disable();
        }

        #region Car Physics

        private void Update()
        {

            TireVisual();
            AudioControl();
            SkidVisual();
        }

        private void FixedUpdate()
        {


            RaycastHit hit;
            Physics.Raycast(_groundRayPoint.position, -transform.up, out hit, _maxRayLenght);

            _carVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
            _carSpeed = _carVelocity.magnitude;

            if (hit.collider)  // for network
            {
                _grounded = true;
            }

            if (!isLocalPlayer)
            {
                //Debug.Log(_carVelocity);
                return;
            }

            if (hit.collider)
            {
                _grounded = true;
                AccelerationLogic();
                TurningLogic();
                FrictionLogic();
                BrakeLogic();

                Debug.DrawLine(_groundRayPoint.position, hit.point, Color.green);

                _rigidbody.centerOfMass = Vector3.zero;
            }
            else
            {
                _grounded = false;

                _rigidbody.drag = 0.1f;
                _rigidbody.centerOfMass = _centerOfMass.localPosition;
                //_rigidbody.centerOfMass = Vector3.zero;
                _rigidbody.angularDrag = 0.1f;
            }
        }

        private void AccelerationLogic()
        {
            _speedValue = _speed * _speedInput * Time.fixedDeltaTime * _multiplicatorValue * _speedCurve.Evaluate(Mathf.Abs(_carVelocity.z) / _scaleValue);

            if (_speedInput > _inputThreshold)
            {
                _rigidbody.AddForceAtPosition(transform.forward * _speedValue, _groundRayPoint.position);
            }

            else if (_speedInput < -_inputThreshold)
            {
                _rigidbody.AddForceAtPosition(transform.forward * _speedValue / 2, _groundRayPoint.position);
            }
        }

        private void TurningLogic()
        {
            _turnValue = _turn * _turnInput * Time.fixedDeltaTime * _multiplicatorValue * _turnCurve.Evaluate(_carSpeed / _scaleValue);

            if (_carVelocity.z > _carVelocityThreshold)
            {
                _rigidbody.AddTorque(transform.up * _turnValue);
            }
            else if (_carVelocity.z < -_carVelocityThreshold)
            {
                _rigidbody.AddTorque(transform.up * -_turnValue);
            }

            _rigidbody.angularDrag = _dragAmount * _driftCurve.Evaluate(Mathf.Abs(_carVelocity.z) / 70);
        }

        private void FrictionLogic()
        {
            _fricValue = _friction * _frictionCurve.Evaluate(_carSpeed / _scaleValue);

            if (_carSpeed > 1)
            {
                _frictionAngle = (-Vector3.Angle(transform.up, Vector3.up) / 90f) + 1;
                _rigidbody.AddForceAtPosition(transform.right * _fricValue * _frictionAngle * _multiplicatorValue / 10 * -_carVelocity.normalized.x, _frictionPoint.position);
            }
        }

        private void BrakeLogic()
        {
            if (_carSpeed < 1)
            {
                _rigidbody.drag = 5f;
            }
            else
            {
                _rigidbody.drag = 0.1f;
            }
        }

        private void TireVisual()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            foreach (Transform tire in _allTiresMesh)
            {
                tire.RotateAround(tire.position, tire.right, _carVelocity.z / _rotatingFactor);
                tire.localPosition = Vector3.zero;
            }

            foreach (Transform tire in _rotatingTiresMesh)
            {
                tire.localRotation = Quaternion.Slerp(tire.localRotation,
                                                      Quaternion.Euler(tire.localRotation.eulerAngles.x, _turnAngle * _turnInput, tire.localRotation.eulerAngles.z),
                                                      _turnDuration);
            }
        }

        private void AudioControl()
        {
            if (_grounded && Mathf.Abs(_carVelocity.x) > _skidThreshold)
                _skidSound.mute = false;

            else
                _skidSound.mute = true;

            _engineSoundHighPitched.pitch = 2 * _engineCurve.Evaluate(_carSpeed / _scaleValue);
            _engineSoundLowPitched.pitch = 2 * _engineCurve.Evaluate(_carSpeed / _scaleValue);
        }

        private void SkidVisual()
        {
            if (_grounded && Mathf.Abs(_carVelocity.x) > _skidThreshold)
            {
                for (int index = 0; index < _skids.Length; index++)
                {
                    _skids[index].trailRenderer.emitting = true;
                    _skids[index].particleSystem.Play();
                }
            }
            else
            {
                for (int index = 0; index < _skids.Length; index++)
                {
                    _skids[index].trailRenderer.emitting = false;
                    _skids[index].particleSystem.Stop();
                }
            }
        }

        #endregion

        private void ForwardBackward(InputAction.CallbackContext context)
        {
            _speedInput = context.ReadValue<float>();
        }

        private void LeftRigth(InputAction.CallbackContext context)
        {
            _turnInput = context.ReadValue<float>();
        }

        public void RaceStart()
        {
            _playerMap.PlayerMovement.Enable();
            _ghost._isInRace = true;
            if (_ghostSaveCoroutine == null)
            {
                _ghostSaveCoroutine = _ghost.GetData();
                StartCoroutine(_ghostSaveCoroutine);
            }
        }

        public void RaceStop()
        {
            _playerMap.PlayerMovement.Disable();
            _ghost._isInRace = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isLocalPlayer)
            {
                return;
            }
            Road roadScript;
            if ((roadScript = other.gameObject.GetComponent<Road>()) != null)
            {
                ValidMapEditor.VehiclePassPoint(roadScript);
            }
        }
    }
    

