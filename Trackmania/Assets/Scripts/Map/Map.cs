using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Union between MapData and MapInfo
/// </summary>
public class Map : MonoBehaviour
{
    #region publicAttributes
    // CamelCase

    public MapInfo info { get; private set; }
    public MapData data { get; private set; }

    #endregion

    #region privateAttributes
    // CamelCase + _ in front


    #endregion

    #region publicMethods
    // CamelCase + start with uppercase

    public Map(MapInfo _mapInfo) {
        info = _mapInfo;
        data = new MapData(info.path);
    }

    #endregion

    #region privateMethods
    // CamelCase + start with uppercase


    #endregion

}
