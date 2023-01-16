using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GhostList
{
    public List<GhostData> GhostPos = new List<GhostData>();
}
[System.Serializable]
public class GhostData
{
    public Vector3 position;
    public Quaternion rotation;

    public GhostData(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
public class Ghost
{
    public float _timeBetweenGetData = 0.15f;
    private string _path;
    private string _json;
    public bool _isInRace = false;
    GhostList ghostList = new GhostList();
    private Transform _transform;
    public Ghost()
    {
    }
    public Ghost(Transform transform, string mapID)
    {
        _transform = transform;
        _path = MapSaver.GetMapDirectory(mapID) + MapSaver.MapGhostInfo;
    }
    public IEnumerator GetData()
    {
        while (_isInRace)
        {
            yield return new WaitForSeconds(_timeBetweenGetData);
            saveData();
        }
    }
    public void RestartData()
    {
        ghostList.GhostPos.Clear();
    }

    private void saveData()
    {
        GhostData saveGhost = new GhostData(_transform.position, _transform.rotation);
        saveGhost.position = _transform.position;
        saveGhost.rotation = _transform.rotation;
        ghostList.GhostPos.Add(saveGhost);
    }
    public void sendGhostData()
    {
        if (ghostList.GhostPos.Count > 0)
        {
            _json = JsonUtility.ToJson(ghostList);
            File.WriteAllText(_path, _json);
        }
    }
    public List<GhostData> loadGhost()
    {
        if (File.Exists(_path))
        {
            string jsonStr = File.ReadAllText(_path);
            GhostList mySampleFile = JsonUtility.FromJson<GhostList>(jsonStr);
            return mySampleFile.GhostPos;
        }

        return null;
    }
}
