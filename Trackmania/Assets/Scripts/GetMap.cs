using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GetMap
{
    public static List<MapInfo> GetEditMap()
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

    public static List<MapInfo> GetLocalMap()
    {
        string[][] directoriesPath = new string[2][];
        string file;

        string folderPath = MapSaver.MapDataPath + MapSaver.Local;
        directoriesPath[0] = Directory.GetDirectories(folderPath);

        folderPath = MapSaver.MapDataPath + MapSaver.Online;
        directoriesPath[1] = Directory.GetDirectories(folderPath);

        List<MapInfo> _allMapInfo = new List<MapInfo>();

        for(int directoryIndex = 0; directoryIndex < directoriesPath.Length; directoryIndex++)
        {
            for (int index = 0; index < directoriesPath[directoryIndex].Length; index++)
            {
                file = File.ReadAllText(directoriesPath[directoryIndex][index] + MapSaver.MapInfo);
                _allMapInfo.Add(JsonConvert.DeserializeObject<MapInfo>(file));
            }
        }

        return _allMapInfo;
    }
}
