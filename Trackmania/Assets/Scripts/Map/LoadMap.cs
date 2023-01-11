using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMap : MonoBehaviour
{
    [SerializeField] private string _targetSceneName;
    [SerializeField] private string _fileName = "mapToLoad";
    [SerializeField] private PopUp _popUp;
    [SerializeField] private Button _createNewMap;

    private static LoadMap _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);

        _instance = this;
    }

    private void Start()
    {
        _createNewMap.onClick.AddListener(() => StartCoroutine(LoadSceneAsync()));
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

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_targetSceneName);
        asyncOperation.allowSceneActivation = false;

        yield return asyncOperation.isDone;

        asyncOperation.allowSceneActivation = true;
    }
}
