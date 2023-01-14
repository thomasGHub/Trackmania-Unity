using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    private MapInfo[] _allCampaignInfo;
    private ListJsonData[] _allCampaignData;

    void Awake()
    {
        MapSaver.CheckMapFolder();
    }

    private void Start()
    {
        StartCoroutine(DownloadCampaign());
    }

    private IEnumerator DownloadCampaign()
    {
        RequestData requestMapInfo = new RequestData(Database.TrackmaniaCampaign, Source.TrackmaniaDB, Collection.MapInfo);
        IEnumerator campaignMapInfo = RequestManager.DownloadingAllData(requestMapInfo, ReceiveMapInfo);

        RequestData requestMapData = new RequestData(Database.TrackmaniaCampaign, Source.TrackmaniaDB, Collection.MapData);
        IEnumerator campaignMapData = RequestManager.DownloadingAllData(requestMapData, ReceiveMapData);

        yield return campaignMapInfo;
        yield return campaignMapData;

        UserAccountManager.DownloadMapEnd.Invoke();
    }

    private void ReceiveMapInfo(string data)
    {
        MultipleMapInfo myDeserializedClass = JsonConvert.DeserializeObject<MultipleMapInfo>(data);
        _allCampaignInfo = myDeserializedClass.AllMapInfo;

        int lenght = _allCampaignInfo.Length;

        string campaignPath = MapSaver.MapDataPath + MapSaver.Campaign;
        string directorypath;
        string json;

        for(int i = 0; i < lenght; i++)
        {
            directorypath = campaignPath + "/" + _allCampaignInfo[i].ID;

            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }

            json = JsonConvert.SerializeObject(_allCampaignInfo[i]);
            File.WriteAllText(directorypath + MapSaver.MapInfo, json);
        }
    }

    private void ReceiveMapData(string data)
    {
        MultipleListJsonData myDeserializedClass = JsonConvert.DeserializeObject<MultipleListJsonData>(data);
        _allCampaignData = myDeserializedClass.AllListJsonData;

        int lenght = _allCampaignInfo.Length;

        string campaignPath = MapSaver.MapDataPath + MapSaver.Campaign;
        string directorypath;
        string json;

        for (int i = 0; i < lenght; i++)
        {
            directorypath = campaignPath + "/" + _allCampaignData[i].ID;

            if (!Directory.Exists(directorypath))
            {
                Directory.CreateDirectory(directorypath);
            }

            json = JsonConvert.SerializeObject(_allCampaignData[i]);
            File.WriteAllText(directorypath + MapSaver.MapBlocks, json);
        }
    }
}
