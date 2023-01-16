using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using PlayFab.MultiplayerModels;
using System.Reflection;

public enum MedalsType { None, Bronze, Silver, Gold, Author}


public class SoloCampaignMenuView : View
{
    public static SoloCampaignMenuView instance;
    public Button MedalsButton;
    public Button WordButton;
    public Button BackButton;

    public Button[] LevelButton;

    public Sprite bronze;
    public Sprite silver;
    public Sprite gold;
    public Sprite author;

    [Header("Leaderboard")]
    public TextMeshProUGUI LeaderboardTitle;
    public GameObject LeaderBoardLocal;
    public GameObject LeaderBoardWorld;
    public ChooseOfflineMapBlockCreator ChooseMap;

    public GameObject rowPrefab;
    public Transform rowsParent;


    public float updateLeaderBoardEverySecond;
    public bool isWorldCategory = false;

    [Header("MapLoading")]
    [SerializeField] private GameObject _mapDataPrefab;
    [SerializeField] private Transform _parentTransform;


    [Header("Debug")]
    public TMP_InputField LeaderboardNameInput;
    public TMP_InputField LeaderboardScoreInput;
    public Button LeaderboardSendDebugButton;

    private List<MapInfo> _allMapInfos;
    private Dictionary<string, CampaignButtonUI> _allCampaignMapBlocks;

    private void Awake()
    {
        instance = this;
    }

    public override void Initialize()
    {
        MedalsButton.onClick.AddListener(() => CategoryChange(false));
        WordButton.onClick.AddListener(() => CategoryChange(true));
        BackButton.onClick.AddListener(() => ViewManager.ShowLast());

        InitializeButtonMap();
        ResetTimer();

        WordButton.onClick.AddListener(() => GetLocalRank());

        LeaderboardSendDebugButton.onClick.AddListener(() => OnSendLeaderboardEntrydebug());

        LoadCampaignMap();

        CategoryChange(false);
    }

    private void LoadCampaignMap()
    {
        _allCampaignMapBlocks = new Dictionary<string, CampaignButtonUI>();
        _allMapInfos = GetMap.GetCampaignMap();

        for(int index = 0; index < _allMapInfos.Count; index++)
        {
            GameObject mapUIBlock = Instantiate(_mapDataPrefab, _parentTransform);
            CampaignButtonUI campaignButton = mapUIBlock.GetComponent<CampaignButtonUI>();
            campaignButton.Init(_allMapInfos[index], index + 1);
            _allCampaignMapBlocks[_allMapInfos[index].ID] = campaignButton;
        }
    }

    public void GetLocalRank()
    {
        int lenght = _allMapInfos.Count;

        for (int index = 0; index < lenght; index++)
        {
            LeaderboardManager.instance.GetLeaderboardAroundPlayer(_allMapInfos[index].ID);
            _allCampaignMapBlocks[_allMapInfos[index].ID].UpdateRankText();
        }

        MouseOnButtonNumber(_allMapInfos[0].ID, _allMapInfos[0].Name);
        DisplayRightLeaderboard(_allMapInfos[0].ID);
    }

    private void ResetTimer()
    {
        for (int number = 0; number < 16; number++)
        {
            if (PlayerPrefs.HasKey("UpdateLeaderboardAt" + number))
            {
                PlayerPrefs.DeleteKey("UpdateLeaderboardAt" + number);
            }
        }
        
    }

    void CategoryChange(bool isWorld)
    {
        isWorldCategory = isWorld;
        if (!isWorld)
        {
            DestroyLeaderboard();
        }

        foreach(string key in _allCampaignMapBlocks.Keys)
        {
            _allCampaignMapBlocks[key].CategoryChange(isWorld);
        }

        LeaderBoardLocal.SetActive(!isWorld);
        LeaderBoardWorld.SetActive(isWorld);
    }

    void InitializeButtonMap()
    {
        for (int i = 0; i < LevelButton.Length; i++)
        {
            int x = i;
            LevelButton[i].onClick.AddListener(delegate { LoadMap(x); });
            LevelButton[i].GetComponent<CampaignButtonUI>().buttonNumber = x + 1;
            LevelButton[i].GetComponent<CampaignButtonUI>().numberText.text = (x + 1).ToString();
        }
        LeaderboardTitle.text = "Leaderboard " + 1;
    }

    void LoadMap(int numberMap)
    {
        Debug.Log($"LoadMap {numberMap}");
    }

    public void MouseOnButtonNumber(string key, string mapName)
    {
        DisplayRightLeaderboard(key);
        if (isWorldCategory)
        {
            Debug.LogWarning("UpdateLeaderBoard" + key);
            //CheckUpdateLeaderboard(key);

            UpdateLocalLeaderboard(key);
            UpdateLeaderboard(key);
        }
        LeaderboardTitle.text = mapName;


    }

    void CheckUpdateLeaderboard(string key)
    {
        //save the time of last updateLeaderboard to update every 10s
        float time = Time.time;

        if (PlayerPrefs.HasKey("UpdateLeaderboardAt" + key))
        {
            //Debug.Log(time + " " + (time - PlayerPrefs.GetFloat("UpdateLeaderboardAt" + number)));
            if (time  -   PlayerPrefs.GetFloat("UpdateLeaderboardAt" + key) > updateLeaderBoardEverySecond)
            {
                PlayerPrefs.SetFloat(("UpdateLeaderboardAt" + key), time);
                UpdateLeaderboard(key);
                Debug.Log("UpdateLeaderboard" + key);
            }
            else
            {
                Debug.Log("updateLocalLeaderboard");
                UpdateLocalLeaderboard(key);

            }
        }
        else
        {
            Debug.Log("Dont have keyTime" +time + " " + (time - PlayerPrefs.GetFloat("UpdateLeaderboardAt" + key)));
            PlayerPrefs.SetFloat(("UpdateLeaderboardAt" + key), time);
            UpdateLeaderboard(key);
            Debug.Log("UpdateLeaderboard" + key);
        }
    }

    void UpdateLeaderboard(string key)
    {
        LeaderboardManager.instance.GetLeaderboard(key);
    }

    void UpdateLocalLeaderboard(string key)
    {
        var mapJson = LeaderboardManager.instance.LoadLeaderboard(key);
        DisplayLeaderboardFromJson(mapJson);
    }


    [ContextMenu("SetNumberCroissant")]
    public void SetNumberButtonCroissant()
    {
        for (int i = 0; i < LevelButton.Length; i++)
        {
            LevelButton[i].GetComponent<CampaignButtonUI>().buttonNumber = i + 1;

        }
    }

    public void DisplayRightLeaderboard(string key)
    {
        MapLeaderboard localLeaderboard =  LeaderboardManager.instance.LoadLeaderboard(key);
        if (localLeaderboard!=null && localLeaderboard.localPlayer!=null && localLeaderboard.localPlayer.playerScore != string.Empty)
        {
            LeaderBoardWorld.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (localLeaderboard.localPlayer.playerRank+1).ToString();
            LeaderBoardWorld.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = localLeaderboard.localPlayer.playerName.ToString();
            LeaderBoardWorld.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = SetScore(localLeaderboard.localPlayer.playerScore);

            LeaderBoardLocal.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SetScore(localLeaderboard.localPlayer.playerScore);
        }
        else
        {
            LeaderBoardWorld.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "__";
            LeaderBoardWorld.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("UserName");
            LeaderBoardWorld.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "__";

            LeaderBoardLocal.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "__ " + PlayerPrefs.GetString("UserName") + " __";
        }


    }

    public void DisplayLeaderboardFromJson(MapLeaderboard mapLeaderboard)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);

        }

        for (int i = 0; i < mapLeaderboard.topPlayer.Count; i++)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            newGo.GetComponent<UILeaderboardEntry>().placeText.text = (i + 1).ToString();
            newGo.GetComponent<UILeaderboardEntry>().nameText.text = mapLeaderboard.topPlayer[i].playerName;
            newGo.GetComponent<UILeaderboardEntry>().SetScore(mapLeaderboard.topPlayer[i].playerScore);
            //newGo.GetComponent<UILeaderboardEntry>().valueText.text = mapLeaderboard.playerScores[i].playerScore;
        }
    }

    public void OnSendLeaderboardEntrydebug()
    {
        LeaderboardManager.instance.SendLeaderboard(LeaderboardNameInput.text, int.Parse(LeaderboardScoreInput.text));
    }


    public void DestroyLeaderboard()
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
    }

    public string SetScore(string _score)
    {
        StringBuilder myString = new StringBuilder();
        if (_score.Length - 8 >= 0)
        {
            myString.Append(_score[_score.Length - 8]);
        }
        else
        {
            myString.Append("0");
        }
        if (_score.Length - 7 >= 0)
        {
            myString.Append(_score[_score.Length - 7]);
        }
        else
        {
            myString.Append("0");
        }
        if (_score.Length - 6 >= 0)
        {
            myString.Append(_score[_score.Length - 6]);
        }
        else
        {
            myString.Append("0");
        }

        myString.Append(".");

        if (_score.Length - 5 >= 0)
        {
            myString.Append(_score[_score.Length - 5]);
        }
        else
        {
            myString.Append("0");
        }
        if (_score.Length - 4 >= 0)
        {
            myString.Append(_score[_score.Length - 4]);
        }
        else
        {
            myString.Append("0");
        }

        myString.Append(":");

        if (_score.Length - 3 >= 0)
        {
            myString.Append(_score[_score.Length - 3]);
        }
        else
        {
            myString.Append("0");
        }
        if (_score.Length - 2 >= 0)
        {
            myString.Append(_score[_score.Length - 2]);
        }
        else
        {
            myString.Append("0");
        }
        if (_score.Length - 1 >= 0)
        {
            myString.Append(_score[_score.Length - 1]);
        }
        else
        {
            myString.Append("0");
        }

        return myString.ToString();
    }



}
