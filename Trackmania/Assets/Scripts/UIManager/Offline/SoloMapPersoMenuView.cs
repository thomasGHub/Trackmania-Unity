using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoloMapPersoMenuView : View
{
    public Button BackButton;

    public override void Initialize()
    {
        BackButton.onClick.AddListener(() => ViewManager.ShowLast());
    }
}
