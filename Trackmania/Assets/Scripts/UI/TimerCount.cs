using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Mirror;
using MirrorBasics;

[System.Serializable]
public class Temps
{
    public int _minutes;
    public int _seconds;
    public int _miliseconds;

    public Temps(int __miliseconds, int __seconds, int __minutes)
    {
        _minutes = __miliseconds;
        _seconds = __seconds;
        _miliseconds = __miliseconds;
    }
    public Temps() { }


    public static bool IsNewTempsBest(Temps newTemps, Temps oldTemps)
    {
        int newTempsInt = TempsToInt(newTemps);
        int oldTempsInt = TempsToInt(oldTemps);


        if (newTempsInt < oldTempsInt || oldTempsInt == 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    public static int TempsToInt(Temps score)
    {
        if (score != null)
        {
            int intScore = score._miliseconds;
            intScore += score._seconds * 1000;
            intScore += score._minutes * 100000;

            return intScore;
        }
        else
        {
            Debug.LogWarning("SCORE NULL");
            return -1;
        }

    }

    public static string TempsToString(Temps score)
    {
        if (score != null)
        {
            return score._minutes + "." + score._seconds + ":" + score._miliseconds;

        }
        else
        {
            return "";
        }


    }

}

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
        _minutes = 0;
        _seconds = 0;
        _miliseconds = 0;

        _secondsTextMesh.text = _seconds.ToString("D2");
        _minutesTextMesh.text = _minutes.ToString("D2");
        _milisecondsTextMesh.text = _miliseconds.ToString("D3");

        _isRunning = true;
    }

    public void ShowUI(bool show)
    {
        Debug.Log("ShowUI" + show + " "+ PlayerNetwork.localPlayer.gameObject.name);
        _minutesTextMesh.gameObject.SetActive(show);
        _secondsTextMesh.gameObject.SetActive(show);
        _milisecondsTextMesh.gameObject.SetActive(show);
    }


    public void Stop()
    {
        _isRunning = false;
    }

    public Temps EndCourse()
    {
        Temps temps = new Temps(0,0,0);
        temps._miliseconds = _miliseconds;
        temps._minutes = _minutes;
        temps._seconds = _seconds;

        return temps;


    }

}
