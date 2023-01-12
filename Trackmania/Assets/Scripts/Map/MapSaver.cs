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
    private static string _mapToPlay = "/mapToPlay.json";
    private static string _local = "/Local";
    private static string _online = "/Online";

    public static string MapDataPath => _mapDataPath;
    public static string MapBlocks => _mapBlocks;
    public static string MapInfo => _mapInfo;
    public static string MapGhostInfo => _mapGhostInfo;
    public static string MapToPlay => _mapToPlay;
    public static string Local => _local;
    public static string Online => _online;

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

    public static void SaveMapInfo(MapInfo mapInfo, bool isLocal)
    {
        string path = GeneratePath(mapInfo.ID, isLocal);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string json = JsonConvert.SerializeObject(mapInfo);
        File.WriteAllText(path + "/" + MapSaver.MapInfo, json);
    }

    public static void SaveMapData(ListJsonData listJsonData, bool isLocal)
    {
        string path = GeneratePath(listJsonData.ID, isLocal);

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

        return null;
    }

    public static string GeneratePath(string mapID, bool isLocal)
    {
        string path;

        if (isLocal)
            path = _mapDataPath + _local + "/" + mapID;
        else
            path = _mapDataPath + _online + "/" + mapID;

        return path;
    }
}
