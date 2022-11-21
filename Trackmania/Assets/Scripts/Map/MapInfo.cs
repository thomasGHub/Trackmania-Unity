using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map general informations (for lists)
/// </summary>
public class MapInfo : MonoBehaviour
{
    #region publicAttributes
    // CamelCase

    public string mapName { get; private set; }

    //path for the MapData
    public string path { get; private set; }

    // id for local maps / storage
    public int localID { get; private set; }

    // id common for everyone
    public int globalID { get; private set; }
    public bool isPublished { get; private set; }

    #endregion

    #region privateAttributes
    // CamelCase + _ in front
    #endregion

    #region publicMethods
    // CamelCase + start with uppercase
    public MapInfo(string _name)
    {
        mapName = _name;
        localID = GenerateLocalID();
        path = GeneratePath(_name);
    }
    #endregion

    #region privateMethods
    // CamelCase + start with uppercase

    private string GeneratePath(string _name)
    {
        if (isPublished)
        {
            return _name + "_" + globalID + ".json";

        }
        else
        return _name + "_" + "l_" + localID + ".json";
        //TODO : Gestion des doublons avec base de donnée 
    }

    private int GenerateLocalID()
    {
        //TODO
        
        return 0;
    }
    private int GenerateGlobalID()
    {
        //TODO
        //get id from data base
        return 0;
    }
    #endregion
}
