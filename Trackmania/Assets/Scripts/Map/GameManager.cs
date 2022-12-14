using Car;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct RoadPoints
{
    public Road Start;
    public Road End;
    public Road[] CheckPoints;
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [Header("Start")]
    [SerializeField] private GameObject _playerPrefab;

    [Header("LoadMap")]
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private RoadData _roadData;
    [SerializeField] private string _nameOfMapFile;

    private Player _player;
    private PlayerMap _playerMap;
    private MapLoader _mapLoader;

    private RoadPoints _roadPoints;
    private Dictionary<Road, bool> _checkPointPassed = new Dictionary<Road, bool>();
    private Dictionary<Type, Action<Road>> _roadToFunction = new Dictionary<Type, Action<Road>>();

    private Transform _lastCheckPointPassed = null;
    public static Transform LastCheckPointPassed => _instance._lastCheckPointPassed;
    public static Transform StartPosition => _instance._roadPoints.Start.transform;

    #region Ghost
    private List<GhostData> _ghosts = new List<GhostData>();
    Ghost ghost = new Ghost();
    [SerializeField]
    private GameObject _ghostPrefab;
    #endregion

    private void Awake()
    {
        if( _instance != null )
        {
            Destroy(this);
            return;
        }
            
        _instance = this;

        _playerMap = new PlayerMap();
        _playerMap.PlayerUX.StartRace.performed += LanchRace;

        _mapLoader = new MapLoader(_roadData);
    }

    private void OnEnable()
    {
        _playerMap.PlayerUX.StartRace.Enable();
    }

    private void OnDisable()
    {
        _playerMap.PlayerUX.StartRace.Disable();
    }

    private void Start()
    {
        _roadPoints = _mapLoader.LoadMap(_nameOfMapFile, _parentTransform);

        Transform startPoint = _roadPoints.Start.transform;
        GameObject _playerCar = GameObject.Instantiate(_playerPrefab, startPoint.position, startPoint.rotation);
        _player = _playerCar.GetComponent<Player>();

        _roadToFunction.Add(_roadData.CheckPoint.GetType(), CheckPointPassed);
        _roadToFunction.Add(_roadData.Goal.GetType(), EndPointPassed);
    }

    private void LanchRace(InputAction.CallbackContext context)
    {
        foreach(Road checkPoint in _roadPoints.CheckPoints)
        {
            _checkPointPassed[checkPoint] = false;
        }

        _player.RaceStart();
        
        if (ghost.loadGhost(ghost._pathMapToLoad) != null)
        {
            Transform startPoint = _roadPoints.Start.transform;
            Instantiate(_ghostPrefab, startPoint.position, startPoint.rotation);
        }
    }

    public static void VehiclePassPoint(Road roadScript)
    {
        _instance._roadToFunction[roadScript.GetType()].Invoke(roadScript);
    }

    private void CheckPointPassed(Road roadScript)
    {
        if (_checkPointPassed[roadScript])
            return;

        _checkPointPassed[roadScript] = true;
        _lastCheckPointPassed = roadScript.transform;
    }

    private void EndPointPassed(Road roadScript)
    {
        foreach (bool value in _checkPointPassed.Values)
        {
            if (!value)
                return;
        }

        _player.RaceStop();
    }

}
