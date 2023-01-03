using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class RequestData
{
    [JsonProperty("database")]
    private string _database { get; set; }
    [JsonProperty("dataSource")]
    private string _dataSource { get; set; }
    [JsonProperty("collection")]
    private string _collection { get; set; }

    public RequestData(Database database, Source source, Collection collection)
    {
        _database = RequestManager.DatabaseInformation.DatabaseToString[database];
        _dataSource = RequestManager.DatabaseInformation.SourceToString[source];
        _collection = RequestManager.DatabaseInformation.CollectionToString[collection];
    }

    public string Stringnify()
    {
        return JsonConvert.SerializeObject(this, Formatting.None);
    }
}


