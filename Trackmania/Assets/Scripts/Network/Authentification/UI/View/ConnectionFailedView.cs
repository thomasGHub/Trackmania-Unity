using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

        PlayerPrefs.SetString("PlayOffline", "false");
    }


    void PlayOfflineClicked()
    {
        PlayerPrefs.SetString("PlayOffline", "true");
        SceneManager.LoadScene("Offline", LoadSceneMode.Single);

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
