using UnityEngine;
using Car;

public class Player : MonoBehaviour
{
    [SerializeField] private CarController _carController;
    [SerializeField] private SpeedoMeter _speedoMeter;


    public void RaceStart()
    {
        _carController.RaceStart();
        _speedoMeter.Launch();
    }
}
