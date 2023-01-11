using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectMenuView : View
{
    [SerializeField] private GameObject _leaveNetwork;

    public Button CreateButton;

    public override void Initialize()
    {


        CreateButton.onClick.AddListener(()=> OnCreate());
    }
    

    void OnCreate()
    {
        _leaveNetwork.SetActive(true);
        ViewManager.Show<CreateMenuView>(true, true);
    }
    
}
