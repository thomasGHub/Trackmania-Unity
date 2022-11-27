using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineCategoriesView : View
{
    public Button SoloButton;
    public Button OnlineButton;
    public Button LanButton;
    public override void Initialize()
    {
        SoloButton.onClick.AddListener(() => ViewManager.Show<SoloMenuView>(remember: true, additive: true));
        OnlineButton.onClick.AddListener(() => ViewManager.Show<OnlineMenuView>(remember: true, additive: true));
        LanButton.onClick.AddListener(() => ViewManager.Show<LanMenuView>(remember: true, additive: true));
    }

    
}
