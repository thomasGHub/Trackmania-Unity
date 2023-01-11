using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class MapLoader
{
    private RoadData _roadData;

    private Dictionary<int, GameObject> _roadPrefabDict;

    public MapLoader(RoadData roadData)
    {
        _roadData = roadData;
    }

    public RoadPoints LoadMap(string _fileName,Transform parent)
    {
        RoadPoints roadPoints = new RoadPoints();

        _roadPrefabDict = _roadData.GenerateDict();

        GameObject gameObject;
        List<Road> checkPointsList = new List<Road>();
        MapInfo mapInfo;
        string path = MapSaver.MapDataPath + "/" + _fileName + ".json";

        string json = File.ReadAllText(path);
        mapInfo = JsonConvert.DeserializeObject<MapInfo>(json);
        //File.Delete(path);

        ListBlockData listBlockData = MapSaver.GetMapBlock(mapInfo.ID);

        foreach (BlockData blockData in listBlockData.blocks)
        {
            gameObject = GameObject.Instantiate(_roadPrefabDict[blockData.id], blockData.position, blockData.rotation, parent);

            if (blockData.id == _roadData.Start.id)
              roadPoints.Start = gameObject.GetComponent<Road>();

            if(blockData.id == _roadData.Goal.id)
                roadPoints.End = gameObject.GetComponent<Road>();

            if(blockData.id == _roadData.CheckPoint.id)
                checkPointsList.Add(gameObject.GetComponent<Road>());            
        }

        roadPoints.CheckPoints = checkPointsList.ToArray();

        return roadPoints;
    }
}
