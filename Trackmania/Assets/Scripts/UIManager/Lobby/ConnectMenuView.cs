using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectMenuView : View
{
    public Button CreateButton;

    public override void Initialize()
    {
        CreateButton.onClick.AddListener(()=> OnCreate());
    }
    

    void OnCreate()
    {
        ViewManager.Show<CreateMenuView>(true, false);
    }
    
}
