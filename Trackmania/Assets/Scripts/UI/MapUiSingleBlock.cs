using Newtonsoft.Json;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUiSingleBlock : MonoBehaviour
{
    [Header("Ui Object")]
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textAuthor;
    [SerializeField] private TextMeshProUGUI _textDate;
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _publishButton;

    private string _mapInfoPath;
    private string _mapBlocksPath;

    private MapInfo _mapInfo;
    
    public void Init(MapInfo mapInfo)
    {
        _mapInfo = mapInfo;
        _mapInfoPath = MapSaver.MapDataPath + "/" + _mapInfo.ID + "/" + MapSaver.MapInfo;
        _mapBlocksPath = MapSaver.MapDataPath + "/" + _mapInfo.ID + "/" + MapSaver.MapBlocks;

        _textName.text = _mapInfo.Name;
        _textAuthor.text = _mapInfo.Author;
        _textDate.text = _mapInfo.DateTime.ToShortDateString();

        _editButton.onClick.AddListener(EditMap);
        _publishButton.onClick.AddListener(PublishMap);
    }

    private void EditMap()
    {
        LoadEditorMap.SwitchSceneAsync(_mapInfo);
    }

    private void PublishMap()
    {
        bool alreadyPublished = _mapInfo.IsPublished;
        RequestData requestDataInfoMap;
        RequestData requestDataBlocksMap;

        _mapInfo.IsPublished = true;
        _mapInfo.IsModified = false;

        string jsonInfoMap = JsonConvert.SerializeObject(_mapInfo);
        File.WriteAllText(_mapInfoPath, jsonInfoMap);

        string jsonBlocksMap = File.ReadAllText(_mapBlocksPath);
        ListJsonData listBlock = JsonUtility.FromJson<ListJsonData>(jsonBlocksMap);

        if (!alreadyPublished)
        {
            requestDataInfoMap = new NewData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapInfo, _mapInfo);
            requestDataBlocksMap = new NewData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapData, listBlock);
        }
        else
        {
            requestDataInfoMap = new UpdatingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapInfo, _mapInfo, new FilterID(_mapInfo.ID));
            requestDataBlocksMap = new UpdatingData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapData, listBlock, new FilterID(_mapInfo.ID));
        }

        RequestManager.UploadingData(requestDataInfoMap, alreadyPublished);
        RequestManager.UploadingData(requestDataBlocksMap, alreadyPublished);
    }
}
