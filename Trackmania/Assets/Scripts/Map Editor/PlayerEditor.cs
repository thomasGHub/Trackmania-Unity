using UnityEngine;
using Car;
using Cinemachine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerEditor : MonoBehaviour
{
    [SerializeField] private VehiculeEditor _carController;
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
        //gameObject.GetComponent<NetworkTransformChild>().OnTeleport(ValidMapEditor.StartPosition.position, Quaternion.identity);
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

        ResetVehicle(ValidMapEditor.StartPosition);

        RaceStart();
    }

    private void ResetVehicle(Transform destination)
    {
        _carController.Teleportation(destination);
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
        Transform respawnPoint = ValidMapEditor.LastCheckPointPassed;

        if (respawnPoint == null)
            RaceRestart();

        else
        {
            ResetVehicle(respawnPoint);
        }
    }

    private void RaceRestart(InputAction.CallbackContext context)
    {
        RaceRestart();
    }


    public Temps RaceFinish()
    {
        return _timerCount.EndCourse();
    }


    public void SetCamPriorityNotLocalPlayer()
    {
        for (int i = 0; i < _allCameras.Length; i++)
        {
            _allCameras[i].Priority = -10;
        }
    }

    public void DisableNotLocalPlayerCar()
    {
        _carController.isLocalPlayer = false;

    }







}