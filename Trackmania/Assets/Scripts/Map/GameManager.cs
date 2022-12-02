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

    private CarController _playerController;

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
        _playerController = _playerCar.GetComponent<CarController>();
        LanchRace();
    }

    private void LanchRace()
    {
        _playerController.RaceStart();
    }
}
