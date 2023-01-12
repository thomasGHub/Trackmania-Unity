using Car;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MirrorBasics;
using Mirror;
using System.Collections;
using Newtonsoft.Json;

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
    //[SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Vector3 _scaleMap;

    [Header("LoadMap")]
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private RoadData _roadData;


    private Player _player;
    private PlayerMap _playerMap;
    private MapLoader _mapLoader;

    private RoadPoints _roadPoints;
    private Dictionary<Road, bool> _checkPointPassed = new Dictionary<Road, bool>();
    private Dictionary<Type, Action<Road>> _roadToFunction = new Dictionary<Type, Action<Road>>();

    private Transform _lastCheckPointPassed = null;
    public static Transform LastCheckPointPassed => _instance._lastCheckPointPassed;
    public static Transform StartPosition => _instance._roadPoints.Start.transform;

    private Temps localBestTemps = new Temps(0,0,0);
    private Temps _currentTemps;


    #region Ghost
    [SerializeField]
    private GameObject _ghostPrefab;

    private Ghost _ghost;
    private IEnumerator _ghostSaveCoroutine;
    private GhostController _ghostController;
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
        _roadPoints = _mapLoader.LoadMap(MapSaver.MapToPlay, _parentTransform);
        _parentTransform.localScale = _scaleMap;

        Transform startPoint = _roadPoints.Start.transform;

        _player = PlayerNetwork.localPlayer.gameObject.GetComponent<Player>();//_playerCar.GetComponent<Player>();
        _player.gameObject.GetComponent<NetworkTransformChild>().OnTeleport(startPoint.position, startPoint.rotation);

        _roadToFunction.Add(_roadData.CheckPoint.GetType(), CheckPointPassed);
        _roadToFunction.Add(_roadData.Goal.GetType(), EndPointPassed);

        _ghost = new Ghost(_player.PlayerCar.transform, _mapLoader.MapInfo.ID);
    }

    public static void LanchRace()
    {
        foreach(Road checkPoint in _instance._roadPoints.CheckPoints)
        {
            _instance._checkPointPassed[checkPoint] = false;
        }

        List<GhostData> ghostData = _instance._ghost.loadGhost();

        if (ghostData != null)
        {
            Transform startPoint = _instance._roadPoints.Start.transform;
            GameObject gameObject = Instantiate(_instance._ghostPrefab, startPoint.position, startPoint.rotation);
            _instance._ghostController = gameObject.GetComponent<GhostController>();
            _instance._ghostController.Init(ghostData);
        }

        _instance._ghost._isInRace = true;
        if (_instance._ghostSaveCoroutine == null)
        {
            _instance._ghostSaveCoroutine = _instance._ghost.GetData();
            _instance.StartCoroutine(_instance._ghostSaveCoroutine);
        }

        _instance._player.RaceStart();
    }

    public static void RaceRestart()
    {
        foreach (Road checkPoint in _instance._roadPoints.CheckPoints)
        {
            _instance._checkPointPassed[checkPoint] = false;
        }

        _instance._ghost.RestartData();
        if(_instance._ghostSaveCoroutine != null)
            _instance._ghostController.Restart(StartPosition);

        _instance._player.RaceStart();
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

        _currentTemps = _player.RaceFinish();

        if (Temps.IsNewTempsBest(_currentTemps, localBestTemps))
        {
            localBestTemps = _currentTemps;
            PlayerNetwork.localPlayer.CmdSendScore(PlayerNetwork.localPlayer.playerIndex, _currentTemps, PlayerNetwork.localPlayer.playerName);
        }

        SavePersonalTime();

        _ghost._isInRace = false;
    }

    private void SavePersonalTime()
    {
        if(MapSaver.SavePersonalTime(_mapLoader.MapInfo, Temps.TempsToInt(_currentTemps)))
        {
            StartCoroutine(SaveWorldRecord());
        }
    }

    private IEnumerator SaveWorldRecord()
    {
        DownloadingData downloadingData = new DownloadingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapInfo,
            new FilterID(_mapLoader.MapInfo.ID), new WorldRecordProjection());
        StartCoroutine(RequestManager.DownloadingSingleData(downloadingData, GetWorldRecord));

        yield return null;
    }

    private void GetWorldRecord(string data)
    {
        SingleWorldRecord mapWorldRecord = JsonConvert.DeserializeObject<SingleWorldRecord>(data);

        if(mapWorldRecord.WorldRecord == null) //Map not publish
        {
            return;
        }

        if(Temps.TempsToInt(_currentTemps) < mapWorldRecord.WorldRecord.Time || mapWorldRecord.WorldRecord.Time == -1)
        {
            MapWorldRecord personalWorldRecord = new MapWorldRecord(PlayerPrefs.GetString("UserName"), Temps.TempsToInt(_currentTemps));
            UpdatingData updatingData = new UpdatingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapInfo, new WorldRecordData(personalWorldRecord), new FilterID(_mapLoader.MapInfo.ID));
            StartCoroutine(RequestManager.UpdatingdData(updatingData));
            MapInfo mapInfo = _mapLoader.MapInfo;
            mapInfo.WorldRecord = personalWorldRecord;
            MapSaver.SaveMapInfo(mapInfo);
        }
    }

    public static void SetPlayerReference(Player __player )
    {
        _instance._player = __player;
    }





}
