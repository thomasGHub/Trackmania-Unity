using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using UnityEngine;


#region Filter
public class Filter
{
    
}

public class FilterID : Filter
{
    [JsonProperty("ID")]
    private string _id;

    public FilterID(string id)
    {
        _id = id;
    }
}
#endregion

#region Update
public class Update
{

}

public class UpdateMapInfo : Update
{
    [JsonProperty("$set")]
    private object _data;

    public UpdateMapInfo(object data)
    {
        _data = data;
    }
}
#endregion

public class Projection
{

}

public class MapDataProjection : Projection
{
    [JsonProperty("ID")]
    private int _id;
    [JsonProperty("blocks")]
    private int _blocks;

    public MapDataProjection()
    {
        _id = 1;
        _blocks = 1;
    }
}


[Serializable]
public class RequestData
{
    [JsonProperty("database")]
    protected string _database { get; set; }
    [JsonProperty("dataSource")]
    protected string _dataSource { get; set; }
    [JsonProperty("collection")]
    protected string _collection { get; set; }



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

public class UpdatingData : RequestData
{
    [JsonProperty("filter")]
    private Filter _filterBy { get; set; }
    [JsonProperty("update")]
    private UpdateMapInfo _updateMapInfo { get; set; }

    public UpdatingData(Database database, Source source, Collection collection, object data , Filter filterBy) : base(database, source, collection)
    {
        _filterBy = filterBy;
        _updateMapInfo = new UpdateMapInfo(data);
    }
}

public class NewData : RequestData
{
    [JsonProperty("document")]
    protected object _data { get; set; }

    public NewData(Database database, Source source, Collection collection, object data) : base(database, source, collection)
    {
        _data = data;
    }
}

public class DownloadingData : RequestData
{
    [JsonProperty("filter")]
    private Filter _filterBy { get; set; }
    [JsonProperty("projection")]
    private Projection _projection { get; set; }

    public DownloadingData(Database database, Source source, Collection collection, Filter filterBy, Projection projection) : base(database, source, collection)
    {
        _filterBy = filterBy;
        _projection = projection;
    }
}


