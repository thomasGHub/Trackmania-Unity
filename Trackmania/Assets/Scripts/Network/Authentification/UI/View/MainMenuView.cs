using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using Mirror;

public class MainMenuView : View
{
    public Button PlayButton;
    public Button CreateButton;
    public Button MarketPLaceButton;
    public Button SettingsButton;
    public Button DeconnectionButton;
    public Button QuitGameButton;

    public override void Initialize()
    {
        PlayButton.onClick.AddListener(() => PlayClicked());
        CreateButton.onClick.AddListener(() => CreateClicked());
        MarketPLaceButton.onClick.AddListener(() => MarketPlaceClicked());
        SettingsButton.onClick.AddListener(() => SettingsClicked());
        DeconnectionButton.onClick.AddListener(() => OnDeconnection());
        QuitGameButton.onClick.AddListener(() => QuitGame());

        OfflineMode();
    }

    private void OfflineMode()
    {
        if (PlayerPrefs.HasKey("PlayOffline") && PlayerPrefs.GetString("PlayOffline") == "true")
        {
            MarketPLaceButton.interactable = false;
            DeconnectionButton.interactable = false;
        }
    }

    void PlayClicked()
    {
        ViewManager.Show<OnlineCategoriesView>();

    }

    void CreateClicked()
    {
        SceneManager.LoadScene("EditMap");
    }

    void MarketPlaceClicked()
    {
        SceneManager.LoadScene("Marketplace");
    }

    void SettingsClicked()
    {
        ViewManager.Show<SettingsMenuView>();
    }

    private void OnDeconnection()
    {
        StartCoroutine(OnDeco());;
    }

    private void QuitGame()
    {
        Application.Quit();
    }


    public IEnumerator OnDeco()
    {
        NetworkManager.singleton.OnDestroySelf();

        PlayerPrefs.DeleteKey("UserName");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.DeleteKey("Custom_Id");

        DestroyFilesRank();
        StartCoroutine(UserAccountManager.instance.AutoConnectAfter());

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Login", LoadSceneMode.Single);
        yield return new WaitForSeconds(1);
        ViewManager.Show<AuthentificationStartView>();
    }


    public void DestroyFilesRank()
    {
        for (int i = 1; i <= 9; i++)
        {
            string path = Application.persistentDataPath + "/Leaderboard" + i + ".json";
            if (File.Exists(path)) 
            {
                File.Delete(path);
            }
        }
        
    }

}
