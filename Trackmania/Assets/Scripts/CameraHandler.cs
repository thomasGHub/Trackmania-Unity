using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] private AnimationCurve _DistCurve;
    [SerializeField] private float _moveValue;

    [Header("Car Rigidbody")]
    [SerializeField] private Rigidbody _carRigidbody;

    private Vector3 _startLocalPosition;

    private void Start()
    {
        _startLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        Vector3 distMovement = _moveValue * _DistCurve.Evaluate(_carRigidbody.velocity.magnitude / 100) * Vector3.forward;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _startLocalPosition + distMovement, 10 * Time.deltaTime);
    }
}