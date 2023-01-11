using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameView : View
{
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI playerLevel;
    public TextMeshProUGUI playerXp;

    public Button GetLeaderboardButton;
    public Button SendLeaderboardButton;
    public Button LogOutButton;

    private void OnEnable()
    {
        //UserProfil.OnProfilDataUpdated.AddListener(ProfilDataUpdated);
    }

    private void OnDisable()
    {
        //UserProfil.OnProfilDataUpdated.RemoveListener(ProfilDataUpdated);
    }

    private void ProfilDataUpdated(ProfilData profilData)
    {
        playerName.text = profilData.playerName;
        playerLevel.text = Mathf.Floor( profilData.level).ToString() ;
        playerXp.text = profilData.xp.ToString();
    }

    public override void Initialize()
    {
        //SendLeaderboardButton.onClick.AddListener(()=> UserAccountManager.instance.SendLeaderboard((new System.Random()).Next(1,100)));
        //GetLeaderboardButton.onClick.AddListener(()=> UserAccountManager.instance.GetLeaderboard());
        LogOutButton.onClick.AddListener(()=> LogOut());
    }

    public void LogOut()
    {
        ViewManager.Show<AuthentificationStartView>();
        PlayerPrefs.DeleteKey("UserName");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.DeleteKey("Custom_Id");
    }
    
}
