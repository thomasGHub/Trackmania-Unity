using Car;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [Header("Start")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Vector3 _offSetSpawn;

    [Header("LoadMap")]
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private RoadData _roadData;
    [SerializeField] private string _nameOfMapFile;

    private Player _player;
    private PlayerMap _playerMap;
    private MapLoader _mapLoader;

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
        _mapLoader.LoadMap(_nameOfMapFile, _parentTransform, out _startPoint);
        _parentTransform.localScale = _offSetSpawn;
        GameObject _playerCar = GameObject.Instantiate(_playerPrefab, _startPoint.position + _offSetSpawn, _startPoint.rotation);
        _player = _playerCar.GetComponent<Player>();

    }

    private void LanchRace(InputAction.CallbackContext context)
    {
        _player.RaceStart();
    }


}
