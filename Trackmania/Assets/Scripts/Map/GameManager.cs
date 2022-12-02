using Car;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [Header("Start")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Vector3 _offSetSpawn;

    private Player _player;

    private void Awake()
    {
        if( _instance != null )
        {
            Destroy(this);
            return;
        }
            
        _instance = this;
    }

    private void Start()
    {
        GameObject _playerCar = GameObject.Instantiate(_playerPrefab, _startPoint.position + _offSetSpawn, _startPoint.rotation);
        _player = _playerCar.GetComponent<Player>();
        LanchRace();
    }

    private void LanchRace()
    {
        _player.RaceStart();
    }
}
