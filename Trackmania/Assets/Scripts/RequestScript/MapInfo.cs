using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class MapInfo
{
    public string ID { get; private set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public DateTime DateTime { get; set; }

    public bool IsPublished { get; set; }
    public bool IsModified { get; set; }

    public MapInfo(string iD, string name, string author)
    {
        ID = iD;
        Name = name;
        Author = author;
        DateTime = DateTime.Now;
        IsPublished = false;
        IsModified = false;
    }
}

public class SingleElement
{
    [JsonProperty("document")]
    public ListJsonData _listJsonData { get; set; }
}

public class MultipleElement
{
    [JsonProperty("documents")]
    public MapInfo[] _allMapInfo { get; set; }
}