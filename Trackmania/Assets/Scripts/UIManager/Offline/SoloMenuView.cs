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
        SoloCampaign.onClick.AddListener(()=> PlayCampaign()) ;
        SoloMapPerso.onClick.AddListener(()=> PlayMapPerso()) ;
    }

    void PlayCampaign()
    {
        ViewManager.Show<SoloCampaignMenuView>(true, false);
        //SoloCampaignMenuView.instance.MouseOnButtonNumber(1);
        SoloCampaignMenuView.instance.GetLocalRank();
    }
    void PlayMapPerso()
    {
        ViewManager.Show<SoloMapPersoMenuView>(true, false);
    }
}
