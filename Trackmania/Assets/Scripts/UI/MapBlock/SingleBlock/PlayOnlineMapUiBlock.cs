using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayOnlineMapUiBlock : MapUiBlock
{
    [SerializeField] protected Button _playButtton;
    [SerializeField] protected TextMeshProUGUI _username;
    [SerializeField] protected TextMeshProUGUI _time;
    [SerializeField] protected TextMeshProUGUI _worldRecord;

    [SerializeField] protected GameObject[] _worldRecordChild;

    public override void Init(MapInfo mapInfo)
    {
        base.Init(mapInfo);

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

    protected virtual void PlayMap()
    {
        //LoadMap.SwitchSceneAsync(_mapInfo);
        string json = JsonConvert.SerializeObject(_mapInfo);
        File.WriteAllText(MapSaver.MapDataPath + MapSaver.MapToPlay, json);

        ViewManager.GetView<CreateMenuView>().ActiveButton();

    }
}
