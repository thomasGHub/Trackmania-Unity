using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EchapMenuView : View
{
    public Button LeaveRoomButton;

    public Trackmania inputActions;



    public override void Initialize()
    {
        LeaveRoomButton.onClick.AddListener(()=> LeaveRoom());
        inputActions = new Trackmania();
        inputActions.UI.Escape.performed += Escaping;
    }

    public void OnEnable()
    {
        if (inputActions!=null)
        {
            inputActions.UI.Escape.Enable();
        }
    }

    public void OnDisable()
    {
        inputActions.UI.Escape.Disable();
    }

    public void LeaveRoom()
    {
        StartCoroutine( UILobby.instance.MainMenu() );
    }


    public void Escaping(InputAction.CallbackContext context)
    {
        ViewManager.Show<InGameView>();
    }




}
