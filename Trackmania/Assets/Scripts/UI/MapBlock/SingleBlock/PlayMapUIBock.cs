using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMapUIBock : MapUiBlock
{
    [SerializeField] private Button _playButtton;

    public override void Init(MapInfo mapInfo)
    {
        base.Init(mapInfo);

        _playButtton.onClick.AddListener(PlayMap);
    }

    private void PlayMap()
    {
        LoadMap.SwitchSceneAsync(_mapInfo);
    }
}
