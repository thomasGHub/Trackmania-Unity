using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoloMenuView : View
{
    public Button SoloPlayButton;
    public override void Initialize()
    {
        SoloPlayButton.onClick.AddListener(()=>SoloPlay()) ; 
    }

    void SoloPlay()
    {
    
    }
}
