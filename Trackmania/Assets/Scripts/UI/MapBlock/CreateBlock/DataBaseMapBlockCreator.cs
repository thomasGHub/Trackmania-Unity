using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataBaseMapBlockCreator : MapUIBlockCreator
{
    // Start is called before the first frame update

    private MapInfo[] _allDataBaseMap;

    void Start()
    {
        StartCoroutine(GetAllMap());
    }

    private IEnumerator GetAllMap()
    {
        RequestData requestData = new RequestData(Database.Trackmania, Source.TrackmaniaDB, Collection.MapInfo);
        IEnumerator databaseRequest = RequestManager.DownloadingAllData(requestData, ReceiveData);
        yield return databaseRequest;

        _allMapInfo = _allDataBaseMap.ToList();

        Init();

        yield return null;
    }

    private void ReceiveData(MapInfo[] data)
    {
        _allDataBaseMap = data;
    }

}
