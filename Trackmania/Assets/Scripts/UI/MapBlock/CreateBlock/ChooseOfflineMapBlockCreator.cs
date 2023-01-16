using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseOfflineMapBlockCreator : MapUIBlockCreator
{
    // Start is called before the first frame update
    void Start()
    {
        _allMapInfo = GetMap.GetLocalMap();
        Init();
    }
}
