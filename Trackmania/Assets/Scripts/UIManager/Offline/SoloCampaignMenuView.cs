using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum MedalsType { None, Bronze, Silver, Gold, Author}


public class SoloCampaignMenuView : View
{
    public static SoloCampaignMenuView instance;
    public Button MedalsButton;
    public Button WordButton;
    public Button BackButton;

    public Button[] LevelButton;

    public Image bronze;
    public Image silver;
    public Image gold;
    public Image author;

    public GameObject LeaderBoardLocal;
    public GameObject LeaderBoardWorld;

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
        CategoryChange(false);
        ResetTimer();
    }

    private void ResetTimer()
    {
        for (int number = 0; number < 16; number++)
        {
            if (PlayerPrefs.HasKey("UpdateLeaderboardAt" + number))
            {
                PlayerPrefs.SetFloat("UpdateLeaderboardAt" + number, 0);
            }
        }
        
    }

    void CategoryChange(bool isWorld)
    {
        for (int i = 0; i < LevelButton.Length; i++)
        {
            LevelButton[i].GetComponent<CampaignButtonUI>().CategoryChange(isWorld);
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

        }
    }

    void LoadMap(int numberMap)
    {
        Debug.Log($"LoadMap {numberMap}");
    }

    public void MouseOnButtonNumber(int buttonNumber)
    {
        //UpdateLeaderborad i
        //Debug.Log(" //UpdateLeaderborad i");
        CheckUpdateLeaderboard(buttonNumber);

    }

    void CheckUpdateLeaderboard(int number)
    {
        //save the time of last updateLeaderboard to update every 10s
        float time = Time.time;
        if (PlayerPrefs.HasKey("UpdateLeaderboardAt" + number))
        {
            Debug.Log(time + " " + (time - PlayerPrefs.GetFloat("UpdateLeaderboardAt" + number)));
            if (time  -   PlayerPrefs.GetFloat("UpdateLeaderboardAt" + number) > 10)
            {
                PlayerPrefs.SetFloat(("UpdateLeaderboardAt" + number), time);
                UpdateLeaderboard(number);
                Debug.Log("UpdateLeaderboard" + number);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(("UpdateLeaderboardAt" + number), time);
            UpdateLeaderboard(number);
            Debug.Log("UpdateLeaderboard" + number);
        }
    }

    void UpdateLeaderboard(int number)
    {
        LeaderboardManager.instance.GetLeaderboard("Leaderboard" + number);
    }




}
