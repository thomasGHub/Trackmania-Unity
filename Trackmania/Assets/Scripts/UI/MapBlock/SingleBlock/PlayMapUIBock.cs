using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayMapUIBock : MapUiBlock
{
    [SerializeField] private Button _playButtton;
    [SerializeField] private string _fileName = "mapToPlay";

    public override void Init(MapInfo mapInfo)
    {
        base.Init(mapInfo);

        _playButtton.onClick.AddListener(PlayMap);
    }

    private void PlayMap()
    {
        //LoadMap.SwitchSceneAsync(_mapInfo);
        string json = JsonConvert.SerializeObject(_mapInfo);
        File.WriteAllText(MapSaver.MapDataPath + "/" + _fileName + ".json", json);

        ViewManager.GetView<CreateMenuView>().ActiveButton();

    }
}
