using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;
using UnityEngine.SceneManagement;

public class EchapMenuView : View
{
    public Button LeaveRoomButton;



    public override void Initialize()
    {
        LeaveRoomButton.onClick.AddListener(()=> LeaveRoom());
    }



    public void LeaveRoom()
    {
        PlayerNetwork.localPlayer.DisconnectGame();
        ViewManager.Show<ConnectMenuView>();
        SceneManager.UnloadSceneAsync("GameMap");
    }



}
