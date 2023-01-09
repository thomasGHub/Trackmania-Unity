using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using Newtonsoft.Json;

public class DataBaseMapUIBlock : MapUiBlock
{
    [Header("Download")]
    [SerializeField] private Button _downloadButton;
    [SerializeField] private Button _updateButton;
    [SerializeField] private TextMeshProUGUI _downloadText;
    [SerializeField] private string _alreadyDownloadString = "Already Download";
    [SerializeField] private string _updateString = "Update";

    public override void Init(MapInfo mapInfo)
    {
        //Already store in local folder == Created by user
        if (Directory.Exists(MapSaver.MapDataPath + MapSaver.Local + "/" + mapInfo.ID))
        {
            Destroy(gameObject);
        }

        base.Init(mapInfo);

        //DataBase version different than stocked version
        if (Directory.Exists(MapSaver.MapDataPath + MapSaver.Online + "/" + mapInfo.ID))
        {
            string json = File.ReadAllText(MapSaver.MapDataPath + MapSaver.Online + "/" + mapInfo.ID + MapSaver.MapInfo);
            MapInfo localMapInfo = JsonConvert.DeserializeObject<MapInfo>(json);

            if(localMapInfo == mapInfo)
            {
                DisableDownload();
                return;               
            }

            _downloadText.text = _updateString;
        }

        _downloadButton.onClick.AddListener(DownloadMap);
    }

    private void DownloadMap()
    {
        StartCoroutine(DownloadingMap());
    }

    private IEnumerator DownloadingMap()
    {
        DownloadingData downloadingData = new DownloadingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapData, new FilterID(_mapInfo.ID), new MapDataProjection());
        Coroutine downloading = StartCoroutine(RequestManager.DownloadingSingleData(downloadingData, GetData));

        yield return downloading;

        DisableDownload();
    }

    private void GetData(ListJsonData listBlockData)
    {
        MapSaver.SaveOnlineMap(listBlockData, _mapInfo);
        DisableDownload();
    }

    private void DisableDownload()
    {
        _downloadButton.interactable = false;
        _downloadText.text = _alreadyDownloadString;
    }
}
