using UnityEngine;
using Car;
using Cinemachine;
using UnityEngine.InputSystem;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField] private CarController _carController;
    [SerializeField] private SpeedoMeter _speedoMeter;
    [SerializeField] private TimerCount _timerCount;

    [Header("Car Camera")]
    [SerializeField] private CinemachineVirtualCamera[] _allCameras;

    private PlayerMap _playerMap;
    private int _currentIndex;
    public static Player instance;


    private void Awake()
    {
        _playerMap = new PlayerMap();
    }
    public override void OnStartClient()
    {
        OnStartSetup();
    }

    private void Start()
    {
        _allCameras[0].Priority = 1;

        _playerMap.PlayerUX.CameraSwitch.performed += CameraSwitch;

        OnStartSetup();
    }

    private void OnDisable()
    {
        _playerMap.PlayerUX.CameraSwitch.Disable();
    }

    private void OnStartSetup()
    {
        if (isLocalPlayer)
        {
            instance = this;
        }
        else
        {
            _timerCount.SetInactive();
            _carController.enabled = false;
        }
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
