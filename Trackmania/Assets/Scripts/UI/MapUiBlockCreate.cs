using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class MapUiBlockCreate : MonoBehaviour
{
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private GameObject _mapDataPrefab;

    [SerializeField, Range(0, 500)] private int _spaceBetweenBlock;

    void Start()
    {
        string mapFolderPath = MapSaver.MapDataPath;
        string[] directoriesPath = Directory.GetDirectories(mapFolderPath);
        MapInfo mapInfo;

        for(int index = 0; index < directoriesPath.Length; index++)
        {
            GameObject mapUI = GameObject.Instantiate(_mapDataPrefab, _parentTransform);
            //RectTransform rectTransform = mapUI.GetComponent<RectTransform>();
            //rectTransform.position = new Vector3((index + 0.5f) * (_spaceBetweenBlock + rectTransform.rect.width) , rectTransform.position.y, rectTransform.position.z);

            string file = File.ReadAllText(directoriesPath[index] + MapSaver.MapInfo);
            mapInfo = JsonConvert.DeserializeObject<MapInfo>(file);
            mapUI.GetComponent<MapUiSingleBlock>().Init(mapInfo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
