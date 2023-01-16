using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseOnlineMapBlockCreator : MapUIBlockCreator
{
    void Start()
    {
        _allMapInfo = GetMap.GetOnlineMap();
        Init();
    }
}
