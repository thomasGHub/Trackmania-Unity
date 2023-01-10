using DG.Tweening.Plugins.Core.PathCore;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class MapSaver
{
    private static string _mapDataPath = Application.persistentDataPath + "/MapData";
    private static string _mapBlocks = "/MapBlocks.json";
    private static string _mapInfo = "/MapInfo.json";
    private static string _local = "/Local";
    private static string _online = "/Online";

    public static string MapDataPath => _mapDataPath;
    public static string MapBlocks => _mapBlocks;
    public static string MapInfo => _mapInfo;
    public static string Local => _local;
    public static string Online => _online;

    public static void CreateNewMap(ListJsonData listBlock, string mapName)
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
        MapInfo mapInfo = new MapInfo(id.ToString(), mapName, PlayerPrefs.GetString("UserName"));
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

    public static ListBlockData GetMapBlock(string id)
    {
        string path = _mapDataPath + _local + "/" + id + _mapBlocks;
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
}