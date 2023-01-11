using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ProfilData
{
    public string playerName;
    public float level;
    public int xp;
}


public class UserProfil : MonoBehaviour
{
    public static UserProfil instance;

    public static UnityEvent<ProfilData> OnProfilDataUpdated = new UnityEvent<ProfilData>();
    //public static UnityEvent<List<PlayerLeaderboardEntry>> OnLeaderboardLevelUpdated = new UnityEvent<List<PlayerLeaderboardEntry>>();

    public ProfilData profilData;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        UserAccountManager.OnUserDataRetrieved.AddListener(UserDataRetrieved);
        UserAccountManager.OnSignInSuccess.AddListener(GetUserData);
        //UserAccountManager.OnLeaderboardRetrieved.AddListener(GetLeaderBoardLevel);
    }

    

    private void OnDisable()
    {
        UserAccountManager.OnUserDataRetrieved.RemoveListener(UserDataRetrieved);
        UserAccountManager.OnSignInSuccess.RemoveListener(GetUserData);
        //UserAccountManager.OnLeaderboardRetrieved.RemoveListener(GetLeaderBoardLevel);



    }

    [ContextMenu("Get Profil Data")]
    void GetUserData()
    {
        Debug.Log("Get User Data");
        UserAccountManager.instance.GetUserData("ProfilData");
    }
    private void UserDataRetrieved(string key, string value)
    {
        if (key == "ProfilData")
        {
            profilData = JsonUtility.FromJson<ProfilData>(value);
            OnProfilDataUpdated.Invoke(profilData);
        }
    }

    [ContextMenu("Set Profil Data")]
    void SetUserData()
    {
        UserAccountManager.instance.SetUserData("ProfilData", JsonUtility.ToJson(profilData));
        UserAccountManager.instance.SetStatistic("Level", Mathf.FloorToInt(profilData.level));
    }




}
