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
        UpdateRankText();

        _button.onClick.AddListener(LoadMap);
    }

    private void OnEnable()
    {
        DisplayMedals();
    }


    private void Start()
    {
        imageMedal.gameObject.SetActive(true);
        rankText.gameObject.SetActive(false);
        top.gameObject.SetActive(false);
    }

    private void DisplayMedals()
    {
        string path = MapSaver.GetMapDirectory(_mapInfo.ID) + MapSaver.MapPersonalTimeInfo;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PersonalMapTime personalMapTime = JsonConvert.DeserializeObject<PersonalMapTime>(json);
            int score = personalMapTime.Time;

            imageMedal.color = new Color(1f, 1f, 1f, 1f);

            if (score < _mapInfo.AuthorMedal)
            {
                imageMedal.sprite = author;
                return;
            }
            else if (score < _mapInfo.GoldMedal)
            {
                imageMedal.sprite = gold;
                return;
            }
            else if (score < _mapInfo.SilverMedal)
            {
                imageMedal.sprite = silver;
                return;
            }
            else if (score < _mapInfo.BronzeMedal)
            {
                imageMedal.sprite = bronze;
                return;
            }
        }

        var tempColor = new Color(1f, 1f, 1f, 0f); ;
        imageMedal.color = tempColor;
        imageMedal.sprite = null;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoloCampaignMenuView.instance.MouseOnButtonNumber(_mapInfo.ID, _mapInfo.Name);
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
        ViewManager.Show<PopUpView>(false, false);
        StartCoroutine(LaunchGame());   
    }


    private IEnumerator LaunchGame()
    {
        string json = JsonConvert.SerializeObject(_mapInfo);
        File.WriteAllText(MapSaver.MapDataPath + MapSaver.MapToPlay, json);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GameMap", LoadSceneMode.Additive);

        yield return asyncOperation.isDone;
        yield return new WaitForSeconds(1f);

        GameManager.GetInstance().gameMode = GameModeFactory.Create(GameModeType.Campaign);
        GameManager.LaunchRace();

        ViewManager.Show<InGameOfflineView>();
        PermananentMenuView.ActivateView(false);
    }

    public void UpdateRankText()
    {
        string path = MapSaver.GetMapDirectory(_mapInfo.ID) + MapSaver.MapLeaderBoardData;
        if (!File.Exists(path))
        {
            return;
        }
        string json = File.ReadAllText(path);
        MapLeaderboard mapLeaderboard = JsonConvert.DeserializeObject<MapLeaderboard>(json);

        if(mapLeaderboard.localPlayer.playerScore != "0")
            rankText.text = (mapLeaderboard.localPlayer.playerRank + 1).ToString();
    }


}
