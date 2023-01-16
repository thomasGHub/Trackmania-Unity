using DG.Tweening.Plugins.Core.PathCore;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MapSaver
{
    private static string _mapDataPath = Application.persistentDataPath + "/MapData";
    private static string _mapBlocks = "/MapBlocks.json";
    private static string _mapInfo = "/MapInfo.json";
    private static string _mapGhostInfo = "/MapGhostInfo.json";
    private static string _mapPersonalTimeInfo = "/MapPersonalTimeInfo.json";
    private static string _mapLeaderBoardData = "/Leaderboard.json";
    private static string _mapToPlay = "/mapToPlay.json";
    private static string _local = "/Local";
    private static string _online = "/Online";
    private static string _campaign = "/Campaign";

    public static string MapDataPath => _mapDataPath;
    public static string MapBlocks => _mapBlocks;
    public static string MapInfo => _mapInfo;
    public static string MapGhostInfo => _mapGhostInfo;
    public static string MapPersonalTimeInfo => _mapPersonalTimeInfo;
    public static string MapLeaderBoardData => _mapLeaderBoardData;
    public static string MapToPlay => _mapToPlay;
    public static string Local => _local;
    public static string Online => _online;
    public static string Campaign => _campaign;

    public static void CreateNewMap(ListJsonData listBlock, string mapName, int bronzeMedal, int silverMedal,int goldMedal,int authorMedal)
    {
        CheckMapFolder();
        string path;
        int id;

        do
        {
            id = UnityEngine.Random.Range(0, 999999);
            path = _mapDataPath + _local + "/" + id;
            Debug.Log(id);

        } while (Directory.Exists(path));

        Directory.CreateDirectory(path);
        MapInfo mapInfo = new MapInfo(id.ToString(), mapName, PlayerPrefs.GetString("UserName"), bronzeMedal, silverMedal, goldMedal, authorMedal);
        listBlock.ID = id.ToString();
        mapInfo.DateTime = DateTime.Now;
        mapInfo.IsModified = true;

        SaveMap(listBlock, mapInfo, true);

    }

    public static void SaveOnlineMap(ListJsonData listBlock, MapInfo mapInfo)
    {
        CheckMapFolder();

        Directory.CreateDirectory(_mapDataPath + _online + "/" + mapInfo.ID);
        SaveMap(listBlock, mapInfo, false);

    }

    public static void SaveMap(ListJsonData listBlock, MapInfo mapInfo, bool local)
    {
        CheckMapFolder();
        if(local)
            mapInfo.IsModified = true;

        string _json;
        string path;

        if(local)
            path = _mapDataPath + _local + "/" + mapInfo.ID;
        else
            path = _mapDataPath + _online + "/" + mapInfo.ID;

        _json = JsonConvert.SerializeObject(listBlock);
        File.WriteAllText(path + _mapBlocks, _json);

        _json = JsonConvert.SerializeObject(mapInfo);
        File.WriteAllText(path + _mapInfo, _json);
    }

    public static void SaveMapInfo(MapInfo mapInfo, bool newMap)
    {
        string path;
        if (newMap)
        {
            path = MapSaver.MapDataPath + MapSaver.Online + "/" + mapInfo.ID;
        }
        else
        {
            path = GetMapDirectory(mapInfo.ID);
        }

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string json = JsonConvert.SerializeObject(mapInfo);
        File.WriteAllText(path + "/" + MapSaver.MapInfo, json);
    }

    public static void SaveMapData(ListJsonData listJsonData, bool newMap)
    {
        string path;
        if (newMap)
        {
            path = MapSaver.MapDataPath + MapSaver.Online + "/" + listJsonData.ID;
        }
        else
        {
            path = GetMapDirectory(listJsonData.ID);
        }

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string json = JsonConvert.SerializeObject(listJsonData);
        File.WriteAllText(path + "/" + MapSaver.MapBlocks, json);
    }
    
    public static void SaveMapToPlay(MapInfo mapInfo)
    {
        string path = _mapDataPath + "/";

        string json = JsonConvert.SerializeObject(mapInfo);
        File.WriteAllText(path + "/" + MapSaver.MapBlocks, json);
    }

    public static bool SavePersonalTime(MapInfo mapInfo, int time)
    {
        string path = GetMapDirectory(mapInfo.ID) + "/" + _mapPersonalTimeInfo;
        string json;
        PersonalMapTime personalMapTime;

        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            personalMapTime = JsonConvert.DeserializeObject<PersonalMapTime>(json);
        }
        else
        {
            personalMapTime = new PersonalMapTime(mapInfo.ID, int.MaxValue);
        }

        if (time < personalMapTime.Time)
        {
            personalMapTime = new PersonalMapTime(mapInfo.ID, time);
            json = JsonConvert.SerializeObject(personalMapTime);
            File.WriteAllText(path, json);
            return true;
        }

        return false;
    }

    public static ListBlockData GetMapBlock(string id)
    {
        string path = GetMapDirectory(id) + "/" + _mapBlocks;
        string jsonStr = File.ReadAllText(path);
        ListJsonData listJsonData = JsonUtility.FromJson<ListJsonData>(jsonStr);
        return listJsonData.DataToUnity();
    }

    public static void CheckMapFolder()
    {
        if(!Directory.Exists(_mapDataPath))
            Directory.CreateDirectory(_mapDataPath);

        if (!Directory.Exists(_mapDataPath + _local))
            Directory.CreateDirectory(_mapDataPath + _local);

        if (!Directory.Exists(_mapDataPath + _online))
            Directory.CreateDirectory(_mapDataPath + _online);

        if (!Directory.Exists(_mapDataPath + _campaign))
            Directory.CreateDirectory(_mapDataPath + _campaign);
    }

    public static string GetMapDirectory(string mapID)
    {
        string path = MapSaver.MapDataPath + MapSaver.Online + "/" + mapID;

        if (Directory.Exists(path))
        {
            return path;
        }

        path = MapSaver.MapDataPath + MapSaver.Local + "/" + mapID;

        if (Directory.Exists(path))
        {
            return path;
        }

        path = MapSaver.MapDataPath + MapSaver.Campaign + "/" + mapID;

        if (Directory.Exists(path))
        {
            return path;
        }

        return null;
    }

    public static bool IsCampaignMap(string mapID)
    {
        string path = MapSaver.MapDataPath + MapSaver.Campaign + "/" + mapID;

        if (Directory.Exists(path))
        {
            return true;
        }

        return false;
    }
}
