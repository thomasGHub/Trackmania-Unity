using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListJsonData
{
    public string ID;
    public List<JsonData> blocks = new List<JsonData>();

    public ListBlockData DataToUnity()
    {
        ListBlockData allBlocks = new ListBlockData(ID);

        foreach (JsonData block in blocks)
        {
            allBlocks.blocks.Add(block.DataToUnity());
        }

        return allBlocks;
    }
}

[Serializable]
public class JsonData
{
    public int id;
    public float[] position = new float[3];
    public float[] rotation = new float[4];

    public JsonData(int _id, Vector3 _position, Quaternion _rotation)
    {
        id = _id;

        position[0] = _position.x;
        position[1] = _position.y;
        position[2] = _position.z;

        rotation[0] = _rotation.x;
        rotation[1] = _rotation.y;
        rotation[2] = _rotation.z;
        rotation[3] = _rotation.w;
    }

    public BlockData DataToUnity()
    {
        Vector3 _positon = new Vector3(position[0], position[1], position[2]);
        Quaternion _rotation = new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
        return new BlockData(id, _positon, _rotation);
    }
}

[Serializable]
public class ListBlockData
{
    public string ID;
    public List<BlockData> blocks = new List<BlockData>();

    public ListBlockData(string _id)
    {
        ID = _id;
    }
}

public class BlockData
{
    public int id;
    public Vector3 position;
    public Quaternion rotation;

    public BlockData(int _id, Vector3 _position, Quaternion _rotation)
    {
        id = _id;
        position = _position;
        rotation = _rotation;
    }
}
