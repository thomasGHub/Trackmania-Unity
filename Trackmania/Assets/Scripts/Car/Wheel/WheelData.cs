using UnityEngine;

namespace Car
{
    [CreateAssetMenu(fileName = "WheelData", menuName = "Wheel/WheelData", order = 1)]
    public class WheelData : ScriptableObject
    {
        [Space(10)]
        [Header("Wheel")]
        [SerializeField] private float _steeringTime = 8f;

        [Space(10)]
        [Header("Suspension")]
        [SerializeField] private float _springTravel = 0.1f;
        [SerializeField] private float _springStiffness = 30;
        [SerializeField] private float _damperStiffness = 10;

        [Space(10)]
        [Header("Grip")]
        [SerializeField] private float _gripFactor = 0.8f;

        public float SteeringTime => _steeringTime;

        public float SpringTravel => _springTravel;
        public float SpringStiffness => _springStiffness;
        public float DamperStiffness => _damperStiffness;

        public float GripFactor => _gripFactor;
    }
}


