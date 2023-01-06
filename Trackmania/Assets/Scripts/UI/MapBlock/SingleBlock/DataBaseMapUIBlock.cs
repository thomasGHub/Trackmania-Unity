using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataBaseMapUIBlock : MapUiBlock
{
    [SerializeField] private Button _downloadButton;

    public override void Init(MapInfo mapInfo)
    {
        base.Init(mapInfo);

        _downloadButton.onClick.AddListener(DownloadMap);
    }

    private void DownloadMap()
    {
        DownloadingData downloadingData = new DownloadingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapData, new FilterID(_mapInfo.ID), new MapDataProjection());
        StartCoroutine(RequestManager.DownloadingSingleData(downloadingData, GetData));
    }

    private void GetData(ListJsonData listBlockData)
    {
        MapSaver.SaveOnlineMap(listBlockData, _mapInfo);
    }
}
