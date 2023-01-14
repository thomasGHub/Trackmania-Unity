using Newtonsoft.Json;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalMapUIBlock : MapUiBlock
{
    [Header("Edit")]
    [SerializeField] private Button _editButton;

    [Header("Publish")]
    [SerializeField] private Button _publishButton;
    [SerializeField] private TextMeshProUGUI _publishText;
    [SerializeField] private string _alreadyPublishText = "No Modification";

    protected string _mapInfoPath;
    protected string _mapBlocksPath;

    public override void Init(MapInfo mapInfo)
    {
        base.Init(mapInfo);

        _mapInfoPath = MapSaver.MapDataPath + MapSaver.Local + "/" + _mapInfo.ID + "/" + MapSaver.MapInfo;
        _mapBlocksPath = MapSaver.MapDataPath + MapSaver.Local + "/" + _mapInfo.ID + "/" + MapSaver.MapBlocks;

        _editButton.onClick.AddListener(EditMap);

        if(_mapInfo.IsPublished && !_mapInfo.IsModified)
        {
            Published();
        }

        _publishButton.onClick.AddListener(PublishMap);
    }

    protected void Published()
    {
        _publishButton.interactable = false;
        _publishText.text = _alreadyPublishText;
    }

    private void EditMap()
    {
        LoadMap.SwitchSceneAsync(_mapInfo);
    }

    protected virtual void PublishMap()
    {
        bool alreadyPublished = _mapInfo.IsPublished;

        _mapInfo.IsPublished = true;
        _mapInfo.IsModified = false;

        //Saving modifications
        string jsonInfoMap = JsonConvert.SerializeObject(_mapInfo);
        File.WriteAllText(_mapInfoPath, jsonInfoMap);

        string jsonBlocksMap = File.ReadAllText(_mapBlocksPath);
        ListJsonData listBlock = JsonUtility.FromJson<ListJsonData>(jsonBlocksMap);

        StartCoroutine(UploadingMap(listBlock, alreadyPublished));
    }

    protected virtual IEnumerator UploadingMap(ListJsonData listBlock, bool alreadyPublished)
    {
        RequestData requestDataInfoMap;
        RequestData requestDataBlocksMap;
        Coroutine first;
        Coroutine second;

        if (!alreadyPublished)
        {
            requestDataInfoMap = new NewData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapInfo, _mapInfo);
            requestDataBlocksMap = new NewData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapData, listBlock);

            first = StartCoroutine(RequestManager.SendingNewData(requestDataInfoMap));
            second = StartCoroutine(RequestManager.SendingNewData(requestDataBlocksMap));
        }
        else
        {
            requestDataInfoMap = new UpdatingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapInfo, _mapInfo, new FilterID(_mapInfo.ID));
            requestDataBlocksMap = new UpdatingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapData, listBlock, new FilterID(_mapInfo.ID));

            first = StartCoroutine(RequestManager.UpdatingdData(requestDataInfoMap));
            second = StartCoroutine(RequestManager.UpdatingdData(requestDataBlocksMap));
        }

        yield return first;
        yield return second;

        Published();
    }
}
