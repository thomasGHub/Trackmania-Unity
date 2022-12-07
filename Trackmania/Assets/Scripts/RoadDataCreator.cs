using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoadDataCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] _roadPrefabs;

    [SerializeField] private PrefabData _prefabData;


    [ContextMenu("CreateData")]
    public void RoadDictionnary()
    {
        Debug.Log("Creating");

        Dictionary<int, GameObject> roadList = new Dictionary<int, GameObject>();
        Road roadScript;

        foreach (GameObject roadPrefab in _roadPrefabs)
        {
            roadScript = roadPrefab.GetComponent<Road>();
            roadList[roadScript.id] = roadPrefab;
        }

        _prefabData.RoadsPrefab = roadList;

        EditorUtility.SetDirty(_prefabData);
    }

    [ContextMenu("ReadData")]
    public void ReadDictionnary()
    {
        Debug.Log("Reading");

        foreach(int key in _prefabData.RoadsPrefab.Keys)
        {
            Debug.Log(key);
        }
    }
}
