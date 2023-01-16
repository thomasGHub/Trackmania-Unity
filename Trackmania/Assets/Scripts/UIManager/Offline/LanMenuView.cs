using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanMenuView : View
{
    public Button LanHostButton;
    public Button LanJoinButton;
    public override void Initialize()
    {
        LanHostButton.onClick.AddListener(() => LanHost());
        LanJoinButton.onClick.AddListener(() => LanJoin());
    }

    void LanHost()
    {

    }

    void LanJoin()
    {

    }
}
