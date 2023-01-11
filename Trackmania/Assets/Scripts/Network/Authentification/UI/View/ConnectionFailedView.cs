using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConnectionFailedView : View
{
    public Button PlayOffline;
    public Button RetryToConnect;
    public Button QuitGame;

    public TextMeshProUGUI Error;


    public override void Initialize()
    {
        PlayOffline.onClick.AddListener(() => PlayOfflineClicked());
        RetryToConnect.onClick.AddListener(() => RetryToConnectClicked());
        QuitGame.onClick.AddListener(()=> QuiGameClicked());
    }


    void PlayOfflineClicked()
    {


    }

    void RetryToConnectClicked()
    {
        UserAccountManager.instance.AutoConnect();
    }

    void QuiGameClicked()
    {
        Application.Quit();
    }


}
