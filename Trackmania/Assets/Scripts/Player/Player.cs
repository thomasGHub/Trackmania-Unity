using UnityEngine;
using Car;
using Cinemachine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private CarController _carController;
    [SerializeField] private SpeedoMeter _speedoMeter;
    [SerializeField] private TimerCount _timerCount;

    [Header("Car Camera")]
    [SerializeField] private CinemachineVirtualCamera[] _allCameras;

    private PlayerMap _playerMap;
    private int _currentIndex;

    private void Awake()
    {
        _playerMap = new PlayerMap();
    }

    private void Start()
    {
        _allCameras[0].Priority = 1;

        _playerMap.PlayerUX.CameraSwitch.performed += CameraSwitch;
        
    }

    private void OnDisable()
    {
        _playerMap.PlayerUX.CameraSwitch.Disable();
    }

    public void RaceStart()
    {
        _carController.RaceStart();
        _speedoMeter.Launch();
        _timerCount.Launch();
        _playerMap.PlayerUX.CameraSwitch.Enable();
    }

    private void CameraSwitch(InputAction.CallbackContext context)
    {
        int cameraIndex = (int)(Mathf.Floor(context.ReadValue<float>())) - 1;

        if(cameraIndex < _allCameras.Length)
        {
            _allCameras[_currentIndex].Priority = -1;
            _allCameras[cameraIndex].Priority = 1;

            _currentIndex = cameraIndex;
        }

    }

    
}
