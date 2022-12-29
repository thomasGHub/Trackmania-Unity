using UnityEngine;
using Car;
using Cinemachine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private CarController _carController;
    [SerializeField] private SpeedoMeter _speedoMeter;
    public TimerCount _timerCount;

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
        _playerMap.PlayerMovement.Respawn.performed += Respawn;
        _playerMap.PlayerMovement.Restart.performed += RaceRestart;

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
        _playerMap.PlayerMovement.Respawn.Enable();
        _playerMap.PlayerMovement.Restart.Enable();
    }

    public void RaceStop()
    {
        _carController.RaceStop();
        _speedoMeter.Stop();
        _timerCount.Stop();
        _playerMap.PlayerUX.CameraSwitch.Disable();
        _playerMap.PlayerMovement.Respawn.Disable();
        _playerMap.PlayerMovement.Restart.Disable();
    }

    public void RaceRestart()
    {

        Ghost ghost = new Ghost();
        ghost.RestartData();
        RaceStop();

        _carController.gameObject.transform.position = GameManager.StartPosition.position;
        _carController.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        RaceStart();
    }

    private void CameraSwitch(InputAction.CallbackContext context)
    {
        int cameraIndex = (int)(Mathf.Floor(context.ReadValue<float>())) - 1;

        if (cameraIndex < _allCameras.Length)
        {
            _allCameras[_currentIndex].Priority = -1;
            _allCameras[cameraIndex].Priority = 1;

            _currentIndex = cameraIndex;
        }
    }

    private void Respawn(InputAction.CallbackContext context)
    {
        Transform respawnPoint = GameManager.LastCheckPointPassed;

        if (respawnPoint == null)
            RaceRestart();

        else
        {
            _carController.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _carController.gameObject.transform.position = respawnPoint.position;
        }
    }

    private void RaceRestart(InputAction.CallbackContext context)
    {
        RaceRestart();
    }


}