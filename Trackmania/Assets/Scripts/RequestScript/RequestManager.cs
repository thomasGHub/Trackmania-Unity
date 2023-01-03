using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        CollectionToString[Collection.Map] = "MapDataCollection";
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
    Map
}
#endregion

public class RequestManager : MonoBehaviour
{
    [SerializeField] private string _databaseURL = "https://data.mongodb-api.com/app/data-xivyv/endpoint/data/v1/action/";

    private DatabaseInformation _databaseInformation = new DatabaseInformation();
    private static RequestManager _instance;

    public static DatabaseInformation DatabaseInformation => _instance._databaseInformation;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);

        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadingData());
        //RequestDatabase request = new RequestDatabase();
        //request.SendRequest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator UploadingData() 
    {
        using (UnityWebRequest request = new UnityWebRequest(_databaseURL + "insertOne", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("api-key", "p0wgTxTbPwPkwoSjGkzIuRtRmkAtFDPxCOd1Tv0qNxXTaXEvPqlRFTgjHqWbo9nw");
            byte[] bodyRaw = File.ReadAllBytes(@"C:\Users\bengel\Desktop\TestDBB\TestDBB\Assets\Upload.json");

            //byte[] bodyRaw = Encoding.UTF8.GetBytes(id);

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

    private IEnumerator DownloadingData()
    {
        using (UnityWebRequest request = new UnityWebRequest(_databaseURL + "findOne", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            //request.SetRequestHeader("Access-Control-Request-Headers", "*");
            //request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("api-key", "p0wgTxTbPwPkwoSjGkzIuRtRmkAtFDPxCOd1Tv0qNxXTaXEvPqlRFTgjHqWbo9nw");

            //byte[] bodyRaw = File.ReadAllBytes(@"C:\Users\Bryan\Desktop\Projet\Unity\TestDBB\Assets\Download.json");

            RequestData requestData = new RequestData(Database.Trackmania, Source.TrackmaniaDB, Collection.Map);
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
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(request.downloadHandler.text);
                Debug.Log("Request : " + request.downloadHandler.text);
                Debug.Log("MapInfo : " + myDeserializedClass.document.ID);
            }
        }
    }
}