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


    public void SetRoomUI(List<string> match)
    {
        roomName.text = $"{match[0]} {match[1]}/12";
        roomID = match[2];
    }

}
