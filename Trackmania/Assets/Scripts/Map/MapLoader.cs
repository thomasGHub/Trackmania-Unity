using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class MapLoader
{
    private RoadData _roadData;
    private Dictionary<int, GameObject> _roadPrefabDict;

    private MapInfo _mapInfo;

    public MapInfo MapInfo => _mapInfo;

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
        string path = MapSaver.MapDataPath + "/" + _fileName;

        string json = File.ReadAllText(path);
        _mapInfo = JsonConvert.DeserializeObject<MapInfo>(json);

        Debug.Log("Map ID : " + _mapInfo.ID);
        //File.Delete(path);

        ListBlockData listBlockData = MapSaver.GetMapBlock(_mapInfo.ID);

        foreach (BlockData blockData in listBlockData.blocks)
        {
            gameObject = GameObject.Instantiate(_roadPrefabDict[blockData.id], blockData.position, blockData.rotation, parent);

            if (blockData.id == _roadData.Start.id)
              roadPoints.Start = gameObject.GetComponent<RoadStart>();

            if(blockData.id == _roadData.Goal.id)
                roadPoints.End = gameObject.GetComponent<Road>();

            if(blockData.id == _roadData.CheckPoint.id)
                checkPointsList.Add(gameObject.GetComponent<Road>());            
        }

        roadPoints.CheckPoints = checkPointsList.ToArray();

        return roadPoints;
    }
}
