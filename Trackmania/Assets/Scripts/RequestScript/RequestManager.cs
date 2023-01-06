using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

#region DataBase Information
public class DatabaseInformation
{
    public Dictionary<Database, string> DatabaseToString = new Dictionary<Database, string>();
    public Dictionary<Source, string> SourceToString = new Dictionary<Source, string>();
    public Dictionary<Collection, string> CollectionToString = new Dictionary<Collection, string>();

    public DatabaseInformation()
    {
        DatabaseToString[Database.Trackmania] = "TrackmaniaDB";

        SourceToString[Source.TrackmaniaDB] = "TrackmaniaDatabase";

        CollectionToString[Collection.MapInfo] = "MapInfoCollection";
        CollectionToString[Collection.MapData] = "MapDataCollection";
    }
}

public enum Database
{
    Trackmania
}

public enum Source
{
    TrackmaniaDB
}

public enum Collection
{
    MapInfo,
    MapData
}
#endregion

public class RequestManager : MonoBehaviour
{
    [SerializeField] private string _databaseURL = "https://data.mongodb-api.com/app/data-xivyv/endpoint/data/v1/action/";
    [SerializeField] private string _apiKey = "p0wgTxTbPwPkwoSjGkzIuRtRmkAtFDPxCOd1Tv0qNxXTaXEvPqlRFTgjHqWbo9nw";

    private DatabaseInformation _databaseInformation = new DatabaseInformation();
    private static RequestManager _instance;
    private string _data = "";

    public static DatabaseInformation DatabaseInformation => _instance._databaseInformation;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);

        _instance = this;
    }

    public static void UploadingData(RequestData requestData, bool alreadyUpdate = false)
    {
        if (alreadyUpdate)
            _instance.StartCoroutine(_instance.UpdatingdData(requestData));
        else
            _instance.StartCoroutine(_instance.SendingNewData(requestData));
    }

    public IEnumerator UpdatingdData(RequestData requestData)
    {
        using (UnityWebRequest request = new UnityWebRequest(_instance._databaseURL + "updateOne", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("api-key", _apiKey);

            string json = requestData.Stringnify();
            Debug.Log(json);
            byte[] bodyRaw = Encoding.ASCII.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection Error");
            }
            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Request Error");
            }
            else
            {
                Debug.Log("Succes");
            }
        }
    }

    public IEnumerator SendingNewData(RequestData requestData)
    {
        using (UnityWebRequest request = new UnityWebRequest(_instance._databaseURL + "insertOne", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("api-key", _apiKey);

            string json = requestData.Stringnify();
            Debug.Log(json);
            byte[] bodyRaw = Encoding.ASCII.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection Error");
            }
            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Request Error");
            }
            else
            {
                Debug.Log("Succes");
            }
        }
    }

    public static IEnumerator DownloadingSingleData(DownloadingData downloadingData, System.Action<ListJsonData> callback = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(_instance._databaseURL + "findOne", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("api-key", "p0wgTxTbPwPkwoSjGkzIuRtRmkAtFDPxCOd1Tv0qNxXTaXEvPqlRFTgjHqWbo9nw");

            string json = downloadingData.Stringnify();
            byte[] bodyRaw = Encoding.ASCII.GetBytes(json);
            Debug.Log(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection Error");
            }
            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Request Error");
            }
            else
            {
                Debug.Log("Succes");
                Debug.Log("Request : " + request.downloadHandler.text);

                SingleElement myDeserializedClass = JsonConvert.DeserializeObject<SingleElement>(request.downloadHandler.text);
                Debug.Log("MapInfo : " + myDeserializedClass._listJsonData.ID);
                callback(myDeserializedClass._listJsonData);
            }
        }
    }

    public static IEnumerator DownloadingAllData(RequestData requestData, System.Action<MapInfo[]> callback = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(_instance._databaseURL + "find", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("api-key", "p0wgTxTbPwPkwoSjGkzIuRtRmkAtFDPxCOd1Tv0qNxXTaXEvPqlRFTgjHqWbo9nw");

            string json = requestData.Stringnify();
            Debug.Log(json);
            byte[] bodyRaw = Encoding.ASCII.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Connection Error");
            }
            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Request Error");
            }
            else
            {
                Debug.Log("Succes");
                _instance._data = request.downloadHandler.text;
                MultipleElement myDeserializedClass = JsonConvert.DeserializeObject<MultipleElement>(request.downloadHandler.text);
                Debug.Log("Request : " + request.downloadHandler.text);
                Debug.Log("MapInfo : " + myDeserializedClass._allMapInfo.Length);

                callback(myDeserializedClass._allMapInfo);
            }
        }
    }
}