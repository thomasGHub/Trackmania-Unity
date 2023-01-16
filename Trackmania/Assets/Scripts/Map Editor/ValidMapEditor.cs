using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class ValidMapEditor : MonoBehaviour
{
    private static ValidMapEditor _instance;
    [SerializeField] private Vector3 _scaleMap;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private RoadData _roadData;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private EditorManager _editorManager;
    private PlayerEditor _player;
    private PlayerMap _playerMap;
    private RoadPoints _roadPoints;
    private Dictionary<Road, bool> _checkPointPassed = new Dictionary<Road, bool>();
    private Dictionary<Type, Action<Road>> _roadToFunction = new Dictionary<Type, Action<Road>>();
    private Transform _lastCheckPointPassed = null;
    public static Transform LastCheckPointPassed => _instance._lastCheckPointPassed;
    public static Transform StartPosition => _instance._roadPoints.Start.transform;
    private List<Road> _roads = new List<Road>();
    private void Awake()
    {
        if( _instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }
    void Start()
    {
        _playerMap = new PlayerMap();
        _editorManager.isTesting = true;
        _instance._roadToFunction.Add(_instance._roadData.CheckPoint.GetType(), _instance.CheckPointPassed);
        _instance._roadToFunction.Add(_instance._roadData.Goal.GetType(), _instance.EndPointPassed);
    }
    public static void LanchRace()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<Road>() != null)
            {
                _instance._roads.Add(go.GetComponent<Road>());
                go.transform.SetParent(_instance._parentTransform);
            }
        }

        _instance._parentTransform.localScale = _instance._scaleMap;

        List<Road> roadList = new List<Road>();
        foreach (Road road in _instance._roads)
        {
            if (road.id == _instance._roadData.Start.id)
                _instance._roadPoints.Start = road;

            if (road.id == _instance._roadData.Goal.id)
                _instance._roadPoints.End = road;

            if (road.id == _instance._roadData.CheckPoint.id)
                roadList.Add(road);
        }
        _instance._roadPoints.CheckPoints = roadList.ToArray();
        Transform startPoint = _instance._roadPoints.Start.transform;
        GameObject goPlayer = GameObject.Instantiate(_instance._playerPrefab, startPoint.position, startPoint.rotation);
        _instance._player = goPlayer.GetComponent<PlayerEditor>();

        foreach (Road checkPoint in _instance._roadPoints.CheckPoints)
        {
            _instance._checkPointPassed[checkPoint] = false;
        }

        _instance._player.RaceStart();
        _instance._playerMap.PlayerUX.StartRace.Enable();
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

        Temps temps = _player.RaceFinish();
        _playerMap.PlayerUX.StartRace.Disable();

        RevertScale();

        _editorManager.setInfo();
    }

    private void RevertScale()
    {
        _instance._parentTransform.localScale = new Vector3(1, 1, 1);

        foreach (Road road in _roads)
        {
            road.transform.SetParent(null);
        }
    }

    private void Reset()
    {
        
    }

    public static void StopTest()
    {
        Destroy(_instance._player.gameObject);

        _instance.RevertScale();

        _instance.Reset();
    }
}
