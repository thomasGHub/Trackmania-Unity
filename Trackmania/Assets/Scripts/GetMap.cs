using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GetMap
{
    public static List<MapInfo> GetLocalMap()
    {
        string mapFolderPath = MapSaver.MapDataPath + MapSaver.Local;
        string[] directoriesPath = Directory.GetDirectories(mapFolderPath);
        List<MapInfo> _allMapInfo = new List<MapInfo>();

        for (int index = 0; index < directoriesPath.Length; index++)
        {

            string file = File.ReadAllText(directoriesPath[index] + MapSaver.MapInfo);
            _allMapInfo.Add(JsonConvert.DeserializeObject<MapInfo>(file));
        }
        return _allMapInfo;
    }
}
