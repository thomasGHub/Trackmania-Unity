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

    public override void Initialize()
    {
        PlayButton.onClick.AddListener(() => PlayClicked());
        CreateButton.onClick.AddListener(() => CreateClicked());
        MarketPLaceButton.onClick.AddListener(() => MarketPlaceClicked());
        SettingsButton.onClick.AddListener(() => SettingsClicked());
        DeconnectionButton.onClick.AddListener(() => OnDeconnection());
    }



    void PlayClicked()
    {
        ViewManager.Show<OnlineCategoriesView>();

    }

    void CreateClicked()
    {
        //Load scene
    }

    void MarketPlaceClicked()
    {

    }

    void SettingsClicked()
    {
        ViewManager.Show<SettingsMenuView>();
    }

    private void OnDeconnection()
    {
        StartCoroutine(OnDeco());
        //SceneManager.LoadScene("Login", LoadSceneMode.Single);
        //ViewManager.Show<AuthentificationStartView>();

        //PlayerPrefs.DeleteKey("UserName");
        //PlayerPrefs.DeleteKey("Password");
        //PlayerPrefs.DeleteKey("Custom_Id");
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
