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
}
public class Ghost
{
    public string _pathMapToLoad = "/testGhost.json";
    public float _timeBetweenGetData = 0.15f;
    private string _json;
    public bool _isInRace = false;
    GhostList ghostList = new GhostList();
    private Transform _transform;
    public Ghost()
    {
    }
    public Ghost(Transform transform)
    {
        _transform = transform;
    }
    public IEnumerator GetData()
    {
        while (_isInRace)
        {
            yield return new WaitForSeconds(_timeBetweenGetData);
            saveData();
        }
        sendGhostData(_pathMapToLoad);
    }
    public void RestartData()
    {
        ghostList.GhostPos.Clear();
    }

    private void saveData()
    {
        GhostData saveGhost = new GhostData();
        saveGhost.position = _transform.position;
        saveGhost.rotation = _transform.rotation;
        ghostList.GhostPos.Add(saveGhost);
    }
    private void sendGhostData(string _pathMapToLoad)
    {
        if (ghostList.GhostPos.Count > 0)
        {
            _json += JsonUtility.ToJson(ghostList);
            Debug.Log("test : " + _json);
            File.WriteAllText(Application.persistentDataPath + _pathMapToLoad, _json);
        }
    }
    public List<GhostData> loadGhost(string _pathMapToLoad)
    {
        string path = Application.persistentDataPath + _pathMapToLoad;
        if(File.Exists(path))
        {
            string jsonStr = File.ReadAllText(path);
            GhostList mySampleFile = JsonUtility.FromJson<GhostList>(jsonStr);
            return mySampleFile.GhostPos;
        }

        return null;
    }
}
