using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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

    public void LoadMap(string nameOfMapFile,Transform parent, out Transform startPoint)
    {
        _roadPrefabDict = _roadData.GenerateDict();

        startPoint = parent; // Have to give a value
        GameObject gameObject;

        string path = Application.persistentDataPath + "/" + nameOfMapFile + ".json";
        string jsonStr = File.ReadAllText(path);
        ListBlock mySampleFile = JsonUtility.FromJson<ListBlock>(jsonStr);

        foreach (jsonData jsonData in mySampleFile.blocks)
        {
            gameObject = GameObject.Instantiate(_roadPrefabDict[jsonData.id], jsonData.position, jsonData.rotation, parent);

            if (jsonData.id == _roadData.Start.id)
              startPoint = gameObject.transform;
        }
    }
}
