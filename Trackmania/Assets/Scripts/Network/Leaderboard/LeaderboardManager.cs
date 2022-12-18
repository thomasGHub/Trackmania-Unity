using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using System;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    public static UnityEvent<string, List<PlayerLeaderboardEntry>> OnLeaderboardRetrieved = new UnityEvent<string, List<PlayerLeaderboardEntry>>();
    public GameObject rowPrefab;
    public Transform rowsParent;



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }
    
    //public void GetLeaderboard(string key)
    //{
    //    PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
    //    {
    //        StatisticName = key,
    //        MaxResultsCount = 5
    //    }, response =>
    //    {
    //        if (response.Leaderboard != null)
    //        {
    //            Debug.Log($"Successful GetLeaderborad");
    //            OnLeaderboardRetrieved.Invoke(key, response.Leaderboard);
    //        }
    //        else { Debug.Log($"Get NULLLL Leaderborad"); }

    //    }, error =>
    //    {
    //        Debug.Log($"Unsuccessful GetLeaderborad");
    //    });
    //}

    public void GetLeaderboard(string key)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = key,
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            newGo.GetComponent<UILeaderboardEntry>().placeText.text = (item.Position + 1).ToString();
            newGo.GetComponent<UILeaderboardEntry>().nameText.text = item.DisplayName;//item.PlayFabId.ToString();
            newGo.GetComponent<UILeaderboardEntry>().valueText.text = item.StatValue.ToString();
            Debug.Log($"{item.Position} {item.PlayFabId} {item.StatValue}");
        }
    }

    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }


    public void SendLeaderboard(string leaderboardName, int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = score

                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Successfull leaderboard sent");
    }

    [ContextMenu("SendLeaderboard")]
    void Send()
    {
        SendLeaderboard("Leaderboard1", 10);


    }

}
