using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMap : MonoBehaviour
{
    [SerializeField] private string _targetSceneName;
    [SerializeField] private string _fileName = "mapToLoad";
    [SerializeField] private PopUp _popUp;

    private static LoadMap _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);

        _instance = this;
    }
    
    public static void SwitchSceneAsync(MapInfo mapInfo)
    {
        _instance.StartCoroutine(_instance.LoadSceneAsync(mapInfo));
    }

    private IEnumerator LoadSceneAsync(MapInfo mapInfo)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_targetSceneName);
        asyncOperation.allowSceneActivation = false;

        string json = JsonConvert.SerializeObject(mapInfo);
        File.WriteAllText(MapSaver.MapDataPath + "/" + _fileName + ".json", json);

        yield return asyncOperation.isDone;

        asyncOperation.allowSceneActivation = true;
    }
}
