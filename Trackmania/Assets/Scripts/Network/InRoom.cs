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
        ListJsonData mySampleFile = JsonUtility.FromJson<ListJsonData>(jsonStr);
        ListBlockData mySambleBlock = mySampleFile.DataToUnity(); 
        foreach (BlockData blockData in mySambleBlock.blocks)
        {
            Instantiate(_idToPrefab[blockData.id], blockData.position, blockData.rotation);
        }
    }


}
