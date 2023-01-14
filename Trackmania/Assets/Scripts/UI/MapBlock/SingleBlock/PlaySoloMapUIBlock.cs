using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySoloMapUIBlock : PlayOnlineMapUiBlock
{
    protected override void PlayMap()
    {
        StartCoroutine(LaunchGame());
    }

    private IEnumerator LaunchGame()
    {
        string json = JsonConvert.SerializeObject(_mapInfo);
        File.WriteAllText(MapSaver.MapDataPath + MapSaver.MapToPlay, json);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GameMap", LoadSceneMode.Additive);

        yield return asyncOperation.isDone;
        yield return new WaitForSeconds(1f);

        GameManager.LaunchRace();

        ViewManager.Show<InGameOfflineView>();
        PermananentMenuView.ActivateView(false);
    }
}
