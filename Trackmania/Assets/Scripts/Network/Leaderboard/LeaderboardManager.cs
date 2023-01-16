using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Events;
using System;
using System.IO;
using UnityEngine.InputSystem;

[System.Serializable]
public class MedalsScore
{
    public int bronzeMedalScore = 100;
    public int silverMedalScore = 80;
    public int goldMedalScore = 60;
    public int authorMedalScore = 40;

    public MedalsScore()
    {
        bronzeMedalScore = 100;
        silverMedalScore = 80;
        goldMedalScore = 60;
        authorMedalScore = 40;
    }

}

[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public string playerScore;
    public int playerRank;
}

[System.Serializable]
public class MapLeaderboard
{
    public string mapName;
    public PlayerScore localPlayer;
    public List<PlayerScore> topPlayer = new List<PlayerScore>();
    public MedalsScore medalsScore = new MedalsScore();
}


public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager instance;

    public static UnityEvent<string, List<PlayerLeaderboardEntry>> OnLeaderboardRetrieved = new UnityEvent<string, List<PlayerLeaderboardEntry>>();
    public static UnityEvent<string, List<PlayerLeaderboardEntry>> OnLeaderboardLocalPlayerRetrieved = new UnityEvent<string, List<PlayerLeaderboardEntry>>();



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        OnLeaderboardRetrieved.AddListener(OnLeaderboardGet);
        OnLeaderboardLocalPlayerRetrieved.AddListener(OnLeaderboardGetLocalRank);

    }

    

    private void OnDisable()
    {
        OnLeaderboardRetrieved.RemoveListener(OnLeaderboardGet);
        OnLeaderboardLocalPlayerRetrieved.RemoveListener(OnLeaderboardGetLocalRank);
    }

    #region Get 

    public void GetLeaderboard(string key)
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest()
        {
            StatisticName = key,
            MaxResultsCount = 5
        }, response =>
        {
            if (response.Leaderboard != null)
            {
                Debug.Log($"Successful GetLeaderborad");
                OnLeaderboardRetrieved.Invoke(key, response.Leaderboard);
            }
            else { Debug.Log($"Get NULLLL Leaderborad"); }

        }, error =>
        {
            Debug.Log($"Unsuccessful GetLeaderborad");
        });
    }


    private void OnLeaderboardGet(string key, List<PlayerLeaderboardEntry> result)
    {
        if (result.Count > 0)
        {
            Debug.Log("Leaderboard : " + result[0]);
        }
        SaveLeaderboard(key, result);
        var mapJson = LoadLeaderboard(key);
        SoloCampaignMenuView.instance.DisplayLeaderboardFromJson(mapJson);
    }

    public void GetLeaderboardAroundPlayer(string key)
    {
        PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = key,
            MaxResultsCount = 1
        }, response =>
        {
            if (response.Leaderboard != null)
            {
                Debug.Log($"Successful Leaderborad of Local Player");
                OnLeaderboardLocalPlayerRetrieved.Invoke(key, response.Leaderboard);
            }
            else { Debug.Log($"Get NULLLL Leaderborad  of Local Player"); }

        }, error =>
        {
            Debug.Log($"Unsuccessful GetLeaderborad  of Local Player");
        });
    }

    private void OnLeaderboardGetLocalRank(string key, List<PlayerLeaderboardEntry> result)
    {
        Debug.Log("ResultCount : " + result.Count);

        for (int i = 0; i < result.Count; i++)
        {
            Debug.Log("result " + result[i]);

        }
        if (result.Count > 0 && result[0].StatValue!=0)
        {
            Debug.Log("Leaderboard Local Rank: " + " Name=" +  result[0].DisplayName + " ID=" + result[0].PlayFabId + " Pos=" + result[0].Position +  " Value=" + result[0].StatValue );
            SaveLocalRank(key, result);
            var mapJson = LoadLeaderboard(key);
            
        }
        
    }

    #endregion

    #region Send

    public void SendLeaderboard(string leaderboardName, int score)
    {
        if (! IsNewScore(leaderboardName, score))
        {
            return;
        }
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = score * (-1)
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardSend, OnError);
    }

    private void OnLeaderboardSend(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Successfull leaderboard sent");
    }


    #endregion

    #region Save Load

    private void SaveLocalRank(string key, List<PlayerLeaderboardEntry> result)
    {
        MapLeaderboard saveMap = LoadLeaderboard(key);
        saveMap.mapName = key;

        PlayerScore localPlayerScore = new PlayerScore();
        localPlayerScore.playerName = result[0].DisplayName;
        localPlayerScore.playerScore = (result[0].StatValue*(-1)).ToString();
        localPlayerScore.playerRank = result[0].Position;

        saveMap.localPlayer = localPlayerScore;

        string _json = JsonUtility.ToJson(saveMap);
        File.WriteAllText(MapSaver.GetMapDirectory(key) + MapSaver.MapLeaderBoardData, _json);
    
    }

    public void SaveLeaderboard(string key, List<PlayerLeaderboardEntry> result)
    {
        MapLeaderboard saveMap = LoadLeaderboard(key);

        saveMap.mapName = key;

        saveMap.topPlayer = new List<PlayerScore>();
        for (int i = 0; i < result.Count; i++)
        {
            PlayerScore playerScore = new PlayerScore();
            playerScore.playerName = result[i].DisplayName;
            playerScore.playerScore = (result[i].StatValue*-(1)).ToString();
            saveMap.topPlayer.Add(playerScore);
        }

        string _json = JsonUtility.ToJson(saveMap);
        File.WriteAllText(MapSaver.GetMapDirectory(key) + MapSaver.MapLeaderBoardData, _json);
    }

    public MapLeaderboard LoadLeaderboard(string key)
    {
        Debug.Log("Load:" + key);
        string path = MapSaver.GetMapDirectory(key) + MapSaver.MapLeaderBoardData;
        if (File.Exists(path))
        {
            string jsonStr = File.ReadAllText(path);
            MapLeaderboard mapLeaderboard = JsonUtility.FromJson<MapLeaderboard>(jsonStr);
            return mapLeaderboard;
        }
        else
        {
            return new MapLeaderboard();
            //return null;
        }
    }


    #endregion

    #region Other


    public bool IsNewScore(string key, int score)
    {
        //open file 
        // compare score 
        // return bool
        if (!File.Exists(MapSaver.GetMapDirectory(key) + MapSaver.MapLeaderBoardData))
        {
            //Debug.LogWarning("Possible errer New Score ");
            return true;
        }
        MapLeaderboard mapJson = LoadLeaderboard(key);
        if (mapJson.localPlayer.playerScore != null &&  mapJson.localPlayer.playerScore != "" &&  int.Parse(mapJson.localPlayer.playerScore ) > score)
        {
            //Debug.LogWarning(mapJson.localPlayer.playerScore + score);
            return true;
        }
        if (mapJson.localPlayer.playerScore != null && mapJson.localPlayer.playerScore == "")
        {
            return true;
        }
        //Debug.LogWarning("Possible errer Not new Score");
        return false;
    }


    void OnError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }


    #endregion




}
