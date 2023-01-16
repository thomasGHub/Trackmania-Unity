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

    public MapWorldRecord WorldRecord { get; set; }


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

        WorldRecord = new MapWorldRecord();

    }

    [JsonConstructor]
    public MapInfo(string iD, string name, string author, int bronzeMedal, int silverMedal, int goldMedal, int authorMedal, MapWorldRecord worldRecord)
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

        WorldRecord = worldRecord;

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

public class SingleWorldRecord
{
    [JsonProperty("document")]
    public MapWorldRecord WorldRecord { get; set; }
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

public class MultipleMapInfo
{
    [JsonProperty("documents")]
    public MapInfo[] AllMapInfo { get; set; }
}

public class MultipleListJsonData
{
    [JsonProperty("documents")]
    public ListJsonData[] AllListJsonData { get; set; }
}

public class PersonalMapTime
{
    public string ID { get;  set; }
    public int Time { get;  set; }

    public PersonalMapTime(string iD, int time)
    {
        ID = iD;
        Time = time;
    }
}

public class MapWorldRecord
{
    public string Author { get; private set; }
    public int Time { get; private set; }

    [JsonConstructor]
    public MapWorldRecord(string author, int time)
    {
        Author = author;
        Time = time;
    }

    public MapWorldRecord()
    {
        Author = null;
        Time = -1;
    }
}