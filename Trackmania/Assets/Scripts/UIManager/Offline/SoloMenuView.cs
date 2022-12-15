using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoloMenuView : View
{
    public Button SoloCampaign;
    public Button SoloMapPerso;
    public Button SoloMapDL;
    public override void Initialize()
    {
        SoloCampaign.onClick.AddListener(()=>SoloPlay()) ; 
    }

    void SoloPlay()
    {
    
    }
}
