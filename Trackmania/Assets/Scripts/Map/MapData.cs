using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Map Data used in the game ( Blocks )
/// </summary>
public class MapData : MonoBehaviour
{
    #region publicAttributes
    // CamelCase
    public List<Block> blocks { get; private set; }


    #endregion

    #region privateAttributes
    // CamelCase + _ in front
    #endregion

    #region publicMethods
    // CamelCase + start with uppercase
    public MapData(string _path)
    {
        blocks = ImportMap(_path);
    }
    public MapData(List<Block> _blocks)
    {
        blocks = _blocks;
    }

    public void SaveMap()
    {
        //TODO
    }
    #endregion

    #region privateMethods
    // CamelCase + start with uppercase

    private List<Block> ImportMap(string _path)
    {
        // TODO
        return new List<Block>();
    }

    
    #endregion
}
