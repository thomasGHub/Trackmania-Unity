using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;
using System.Linq;
using UnityEngine.InputSystem;

public class InGameOfflineView : View
{

    

    public Trackmania inputActions;

    private void Awake()
    {
        inputActions = new Trackmania();
    }

    public override void Initialize()
    {
       
        inputActions.UI.Escape.performed += Escaping;
    }


    public void OnEnable()
    {
        inputActions.UI.Escape.Enable();
    }

    public void OnDisable()
    {
        inputActions.UI.Escape.Disable();
    }




    public void Escaping(InputAction.CallbackContext context)
    {
        ViewManager.Show<EchapMenuOfflineView>();

    }
}
