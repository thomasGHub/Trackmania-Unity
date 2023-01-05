using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Mirror;

public class DebugSystem : MonoBehaviour
{
    public TextMeshProUGUI ping;
    public TextMeshProUGUI Fps;

    public TextMeshProUGUI logMessage;
    public Button Clear;

    void Start()
    {
        Clear.onClick.AddListener(() => OnClear());
    }

    void Update()
    {
        ping.text = "Ping " + Math.Round(NetworkTime.rtt * 1000).ToString();
        Fps.text = "Fps " + (1/ Time.smoothDeltaTime).ToString();
    }



    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logMessage.text += "<#FFFFFF>" + logString + "<#0036FF>" + stackTrace + "<#06B000>" + type + "</color>" + "\n" + "\n"; 
    }


    public void OnClear()
    {
        logMessage.text = "";
    }


}
