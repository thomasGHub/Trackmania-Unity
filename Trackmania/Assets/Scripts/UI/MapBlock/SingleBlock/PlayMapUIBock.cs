using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayMapUIBock : MapUiBlock
{
    [SerializeField] private Button _playButtton;
    [SerializeField] private TextMeshProUGUI _username;
    [SerializeField] private TextMeshProUGUI _time;
    [SerializeField] private TextMeshProUGUI _worldRecord;

    [SerializeField] private GameObject[] _worldRecordChild;

    public override void Init(MapInfo mapInfo)
    {
        base.Init(mapInfo);

        Debug.Log("Author : " + mapInfo.WorldRecord.Author + " , Time : " + mapInfo.WorldRecord.Time);
        if(mapInfo.WorldRecord.Author != null)
        {
            _username.text = mapInfo.WorldRecord.Author;
            _time.text = Temps.IntToString(mapInfo.WorldRecord.Time);
        }
        else
        {
            _worldRecord.text = "No World Record";
            foreach(GameObject gameObject in _worldRecordChild)
            {
                gameObject.SetActive(false);
            }
        }
        

        _playButtton.onClick.AddListener(PlayMap);
    }

    private void PlayMap()
    {
        //LoadMap.SwitchSceneAsync(_mapInfo);
        string json = JsonConvert.SerializeObject(_mapInfo);
        File.WriteAllText(MapSaver.MapDataPath + MapSaver.MapToPlay, json);

        ViewManager.GetView<CreateMenuView>().ActiveButton();

    }
}
