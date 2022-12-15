using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MirrorBasics;

public class RoomX : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    public Button joinRoom;
    public string roomID;

    public void Start()
    {
        joinRoom.onClick.AddListener(() => OnClickJoinRoom());
    }


    void OnClickJoinRoom()
    {
        PlayerNetwork.localPlayer.JoinGame(roomID);
    }


}
