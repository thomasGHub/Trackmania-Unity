using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class ControllerCar : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float mirror;

    private PlayerMap _playerMap;
    private float motor;
    private float steering;

    private void Awake()
    {
        _playerMap = new PlayerMap();
    }

    void Start()
    {
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

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    public void FixedUpdate()
    {

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                Debug.Log(motor);
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void ForwardBackward(InputAction.CallbackContext context)
    {
        motor = mirror * maxMotorTorque * context.ReadValue<float>();
    }

    public void LeftRigth(InputAction.CallbackContext context)
    {
        steering = maxSteeringAngle * context.ReadValue<float>();
    }
}