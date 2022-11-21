using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSaveManager : MonoBehaviour
{
    #region publicAttributes
    // CamelCase

    public List<MapInfo> mapInfoList;
    #endregion

    #region privateAttributes
    // CamelCase + _ in front
    #endregion

    #region publicMethods
    // CamelCase + start with uppercase
    public void Start()
    {
       mapInfoList =  GenerateMapInfoList();
    }


    public void UpdateMapInfoList()
    {
        // TODO get new maps on database and add them to mapInfoList
    }

    public void SaveMap(Map _map)
    {
        //todo

        //save mapinfo

        //save mapData
    }

    #endregion

    #region privateMethods
    // CamelCase + start with uppercase
    private List<MapInfo> GenerateMapInfoList()
    {
        List<MapInfo> _mapInfoList = new List<MapInfo>();
        //TODO : Foreach file in /MapInfo , create the MapInfo, add to the list and return the list

        return _mapInfoList;
    }
    #endregion
}
