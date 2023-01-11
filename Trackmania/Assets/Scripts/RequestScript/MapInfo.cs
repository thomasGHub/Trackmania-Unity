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

    public int BronzeMedal { get; set; }
    public int SilverMedal { get; set; }
    public int GoldMedal { get; set; }
    public int AuthorMedal { get; set; }


    public MapInfo(string iD, string name, string author, int bronzeMedal, int silverMedal, int goldMedal, int authorMedal)
    {
        ID = iD;
        Name = name;
        Author = author;
        DateTime = DateTime.Now;
        IsPublished = false;
        IsModified = false;
        BronzeMedal = bronzeMedal;
        SilverMedal = silverMedal;
        GoldMedal = goldMedal;
        AuthorMedal = authorMedal;

    }

    public static bool operator != (MapInfo a, MapInfo b)
    {
        return a.ID != b.ID || a.Name != b.Name || a.Author != b.Author
            || a.DateTime != b.DateTime || a.IsPublished != b.IsPublished
            || a.IsModified != b.IsModified;
    }

    public static bool operator == (MapInfo a, MapInfo b)
    {
        return a.ID == b.ID && a.Name == b.Name && a.Author == b.Author
            && a.DateTime == b.DateTime && a.IsPublished == b.IsPublished
            && a.IsModified == b.IsModified;
    }
}

public class SingleListJsonData
{
    [JsonProperty("document")]
    public ListJsonData ListJsonData { get; set; }
}

public class SingleMapInfo
{
    [JsonProperty("document")]
    public MapInfo MapInfo { get; set; }
}

public class MultipleElement
{
    [JsonProperty("documents")]
    public MapInfo[] _allMapInfo { get; set; }
}