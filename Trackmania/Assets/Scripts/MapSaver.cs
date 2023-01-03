using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class MapSaver
{
    private static string _mapDataPath = Application.persistentDataPath + "/MapData";
    private static string _mapBlock = "/MapBlock.json";
    private static string _mapInfo = "/MapInfo.json";

    public static string MapDataPath => _mapDataPath;
    public static string MapBlock => _mapBlock;
    public static string MapInfo => _mapInfo;

    public static void SaveMap(ListBlock listBlock, string mapName, bool newFile = true)
    {
        CheckMapFolder();

        int id;
        string path;
        string _json;
        do
        {

            id = Random.Range(0, 999999);
            path = _mapDataPath + "/" + id;
            Debug.Log(id);

        } while (Directory.Exists(path));

        Directory.CreateDirectory(path);

        _json = JsonUtility.ToJson(listBlock);
        File.WriteAllText(path + _mapBlock, _json);
        MapInfo mapInfo = new MapInfo(id.ToString(), mapName, PlayerPrefs.GetString("UserName"));

        _json = JsonConvert.SerializeObject(mapInfo);
        File.WriteAllText(path + _mapInfo, _json);
    }

    public static ListBlock GetMapBlock(string id)
    {
        string path = _mapDataPath + "/" + id + _mapBlock;
        string jsonStr = File.ReadAllText(path);
        return JsonUtility.FromJson<ListBlock>(jsonStr);
    }

    public static void CheckMapFolder()
    {
        if(!Directory.Exists(_mapDataPath))
            Directory.CreateDirectory(_mapDataPath);
    }
}
