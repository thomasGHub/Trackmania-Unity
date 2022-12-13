using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _minutesTextMesh;
    [SerializeField] private TextMeshProUGUI _secondsTextMesh;
    [SerializeField] private TextMeshProUGUI _milisecondsTextMesh;

    private int _minutes;
    private int _seconds;
    private int _miliseconds;

    private bool _isRunning = false;

    private void Awake()
    {
        _minutes = 0;
        _seconds = 0;
        _miliseconds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRunning)
            return;

        _miliseconds += Mathf.FloorToInt(Time.deltaTime * 1000);

        if(_miliseconds >= 1000)
        {
            ++_seconds;
            _secondsTextMesh.text = _seconds.ToString("D2");

            _miliseconds -= 1000;


            if (_seconds >= 60)
            {
                ++_minutes;
                _minutes.ToString("D2");
                _minutesTextMesh.text = _minutes.ToString("D2");
                _seconds -= 60;
            }
        }

        _milisecondsTextMesh.text = _miliseconds.ToString("D3");
    }

    public void Launch()
    {
        _isRunning = true;
    }
}
