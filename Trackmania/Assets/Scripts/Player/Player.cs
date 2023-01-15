using UnityEngine;
using Car;
using Cinemachine;
using UnityEngine.InputSystem;
using MirrorBasics;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private CarController _carController;
    [SerializeField] private SpeedoMeter _speedoMeter;
    [SerializeField] private GameObject _UI;
    [SerializeField] private CountDown _countDownScript;
    public TimerCount _timerCount;

    [Header("Car Camera")]
    [SerializeField] private CinemachineVirtualCamera[] _allCameras;

    private PlayerMap _playerMap;
    private int _currentIndex;
    private IEnumerator _countDown = null;
    public CarController PlayerCar => _carController;

    private void Awake()
    {
        _playerMap = new PlayerMap();
    }

    private void Start()
    {
        //_allCameras[0].Priority = 1;

        _playerMap.PlayerUX.CameraSwitch.performed += CameraSwitch;
        _playerMap.PlayerMovement.Respawn.performed += Respawn;
        _playerMap.PlayerMovement.Restart.performed += RaceRestart;
    }

    private void OnDisable()
    {
        _playerMap.PlayerUX.CameraSwitch.Disable();
    }

    private void OnDestroy()
    {
        _playerMap.PlayerMovement.Restart.Disable();
    }

    public void RaceStart()
    {
        _carController.RaceStart();
        _speedoMeter.Launch();
        _timerCount.Launch();
        _playerMap.PlayerUX.CameraSwitch.Enable();
        _playerMap.PlayerMovement.Respawn.Enable();
        if (GameManager.GetInstance().isMulti)
        {
            if (PlayerNetwork.localPlayer.currentMatch.gameMode.type != GameModeType.Rounds)
                _playerMap.PlayerMovement.Restart.Enable();
        }
    }

    public void RaceStop()
    {
        _carController.RaceStop();
        _speedoMeter.Stop();
        _timerCount.Stop();
        _playerMap.PlayerUX.CameraSwitch.Disable();
        _playerMap.PlayerMovement.Respawn.Disable();

        if(GameManager.GetInstance().isMulti)
        {
            if (PlayerNetwork.localPlayer.currentMatch.gameMode.type == GameModeType.Rounds)
                _playerMap.PlayerMovement.Restart.Disable();
        }
        
            
    }

    public void RaceRestart()
    {
        GameManager.RaceRestart();

        RaceStop();

        ResetVehicle(GameManager.StartPosition);
    }

    public void ResetVehicle(Transform destination)
    {
        if (destination == null)
            destination = _carController.transform;

        _carController.Teleportation(destination);
    }

    private void CameraSwitch(InputAction.CallbackContext context)
    {
        int cameraIndex = (int)(Mathf.Floor(context.ReadValue<float>())) - 1;

        if (cameraIndex < _allCameras.Length)
        {
            _allCameras[_currentIndex].Priority = -1;
            _allCameras[cameraIndex].Priority = 2;

            _currentIndex = cameraIndex;
        }
    }

    private void Respawn(InputAction.CallbackContext context)
    {
        Transform respawnPoint = GameManager.LastCheckPointPassed;

        if (respawnPoint == null)
        {
            if (GameManager.GetInstance().isMulti)
            {
                if (PlayerNetwork.localPlayer.currentMatch.gameMode.type == GameModeType.Rounds)
                {
                    ResetVehicle(GameManager.StartPosition);
                    return;
                }
            }

            RaceRestart();
        }
            

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
        for (int i = 0; i <_allCameras.Length; i++)
        {
            _allCameras[i].Priority = -10;
        }
    }

    public void SetCamPriorityLocalPlayer()
    {
        _allCameras[0].Priority = 1;
        _allCameras[1].Priority = 0;
        _allCameras[2].Priority = 0;

    }

    public void DisableNotLocalPlayerCar()
    {
        _carController.isLocalPlayer = false;

    }

    public void StartCountDown()
    {
        _UI.SetActive(true);
        if(_countDown != null)
        {
            StopCoroutine(_countDown);
        }

        _countDown = _countDownScript.CountDownStart();
        StartCoroutine(_countDown);
    }

 





}