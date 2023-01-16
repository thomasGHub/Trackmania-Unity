using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapUIBlockCreator : MonoBehaviour
{
    [Header("Instantiate Map UI")]
    [SerializeField] protected Transform _parentTransform;
    [SerializeField] protected GameObject _mapDataPrefab;

    protected List<MapInfo> _allMapInfo = new List<MapInfo>();

    protected virtual void Init()
    {
        foreach(MapInfo mapInfo in _allMapInfo)
        {
            GameObject mapUIBlock = GameObject.Instantiate(_mapDataPrefab, _parentTransform);
            mapUIBlock.GetComponent<MapUiBlock>().Init(mapInfo);
        }
    }
}
