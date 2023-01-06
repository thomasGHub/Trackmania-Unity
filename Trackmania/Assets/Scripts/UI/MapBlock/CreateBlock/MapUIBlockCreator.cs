using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapUIBlockCreator : MonoBehaviour
{
    [SerializeField] protected Transform _parentTransform;
    [SerializeField] protected GameObject _mapDataPrefab;

    [SerializeField, Range(0, 500)] protected int _spaceBetweenBlock;

    protected List<MapInfo> _allMapInfo = new List<MapInfo>();

    protected void Init()
    {
        foreach(MapInfo mapInfo in _allMapInfo)
        {
            GameObject mapUIBlock = GameObject.Instantiate(_mapDataPrefab, _parentTransform);
            mapUIBlock.GetComponent<MapUiBlock>().Init(mapInfo);
        }
    }
}
