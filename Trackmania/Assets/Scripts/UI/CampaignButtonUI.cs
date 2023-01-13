using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Text;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.IO;

public class CampaignButtonUI : MonoBehaviour, IPointerEnterHandler
{
    [Header("MedalsSprite")]
    [SerializeField] private Sprite bronze;
    [SerializeField] private Sprite silver;
    [SerializeField] private Sprite gold;
    [SerializeField] private Sprite author;

    public int buttonNumber;
    public TextMeshProUGUI numberText;
    public Image imageMedal;
    public TextMeshProUGUI top;
    public TextMeshProUGUI rankText;
    public Button _button;

    private MapInfo _mapInfo;

    public void Init(MapInfo mapInfo, int index)
    {
        _mapInfo = mapInfo;
        numberText.text = index.ToString();
        buttonNumber = index;
        DisplayMedals();

        _button.onClick.AddListener(LoadMap);
    }


    private void Start()
    {
        imageMedal.gameObject.SetActive(true);
        rankText.gameObject.SetActive(false);
        top.gameObject.SetActive(false);
    }

    private void DisplayMedals()
    {
        Debug.LogWarning("Leaderboard" + (buttonNumber));
        MapLeaderboard localLeaderboard = LeaderboardManager.instance.LoadLeaderboard("Leaderboard" + (buttonNumber));
        if (localLeaderboard != null && localLeaderboard.localPlayer != null && localLeaderboard.localPlayer.playerScore != string.Empty)
        {
            Debug.LogWarning("LeaderBoard Score : " + int.Parse(localLeaderboard.localPlayer.playerScore));
            var tempColor = new Color(1f, 1f, 1f, 1f); ;
            imageMedal.color = tempColor;
            if (int.Parse(localLeaderboard.localPlayer.playerScore) < localLeaderboard.medalsScore.authorMedalScore)
            {
                imageMedal.sprite = author;
            }
            else if (int.Parse(localLeaderboard.localPlayer.playerScore) < localLeaderboard.medalsScore.goldMedalScore)
            {
                imageMedal.sprite = gold;

            }
            else if (int.Parse(localLeaderboard.localPlayer.playerScore) < localLeaderboard.medalsScore.silverMedalScore)
            {
                imageMedal.sprite = silver;

            }
            else if (int.Parse(localLeaderboard.localPlayer.playerScore) < localLeaderboard.medalsScore.bronzeMedalScore)
            {
                imageMedal.sprite = bronze;

            }
            else
            {
                tempColor = new Color(1f, 1f, 1f, 0f); ;
                imageMedal.color = tempColor;
                imageMedal.sprite = null;
            }

        }
        else
        {
            var tempColor = new Color(1f, 1f, 1f, 0f); ;
            imageMedal.color = tempColor;
            imageMedal.sprite = null;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoloCampaignMenuView.instance.MouseOnButtonNumber(buttonNumber);
    }


    public void ApplyRankButton(Image _image)
    {
        imageMedal = _image;
    }


    public void CategoryChange(bool isWorld)
    {
        if (isWorld)
        {
            imageMedal.gameObject.SetActive(false);
            rankText.gameObject.SetActive(true);
            top.gameObject.SetActive(true);
        }
        else
        {
            imageMedal.gameObject.SetActive(true);
            rankText.gameObject.SetActive(false);
            top.gameObject.SetActive(false);
        }
    }

    private void LoadMap()
    {
        StartCoroutine(LaunchGame());   
    }
    
    private IEnumerator LaunchGame()
    {
        string json = JsonConvert.SerializeObject(_mapInfo);
        File.WriteAllText(MapSaver.MapDataPath + MapSaver.MapToPlay, json);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GameMap", LoadSceneMode.Additive);

        yield return asyncOperation.isDone;
        yield return new WaitForSeconds(1f);

        GameManager.LanchRace();

        ViewManager.Show<InGameOfflineView>();
        PermananentMenuView.ActivateView(false);
    }


}
