using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InRoom : MonoBehaviour
{
    public static InRoom instance;
    private string _pathMapToLoad = "/test.json";
    private Dictionary<int, GameObject> _idToPrefab;
    [SerializeField] private RoadData _roadData;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _idToPrefab = _roadData.GenerateDict();
    }

    void Update()
    {
        
    }

    public void LoadFileMap()
    {
        string path = Application.persistentDataPath + _pathMapToLoad;
        string jsonStr = File.ReadAllText(path);
        ListBlock mySampleFile = JsonUtility.FromJson<ListBlock>(jsonStr);
        foreach (jsonData jsonData in mySampleFile.blocks)
        {
            Instantiate(_idToPrefab[jsonData.id], jsonData.position, jsonData.rotation);
        }
    }


}
