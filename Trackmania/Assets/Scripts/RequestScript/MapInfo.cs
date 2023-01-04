using Newtonsoft.Json;
using System;
using UnityEngine;

public class MapInfo
{
    [JsonProperty("_id")]
    public string ID { get; private set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public DateTime DateTime { get; set; }

    public MapInfo(string iD, string name, string author)
    {
        ID = iD;
        Name = name;
        Author = author;
        DateTime = DateTime.Now;
    }
}

public class Root
{
    public MapInfo document { get; set; }
}