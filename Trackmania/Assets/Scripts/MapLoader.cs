using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapLoader
{
    private RoadData _roadData;

    private Dictionary<int, GameObject> _roadPrefabDict;

    public MapLoader(RoadData roadData)
    {
        _roadData = roadData;
    }

    public void RoadDictionnary()
    {
        _roadPrefabDict = new Dictionary<int, GameObject>();
        Road roadScript;

        foreach (GameObject roadPrefab in _roadData.AllRoadsPrefabs)
        {
            roadScript = roadPrefab.GetComponent<Road>();
            _roadPrefabDict[roadScript.id] = roadPrefab;
        }
    }

    public RoadPoints LoadMap(string ID,Transform parent)
    {
        RoadPoints roadPoints = new RoadPoints();

        _roadPrefabDict = _roadData.GenerateDict();

        GameObject gameObject;
        List<Road> checkPointsList = new List<Road>();

        ListBlockData listBlockData = MapSaver.GetMapBlock(ID);

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
