using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateMenuView : View
{
    public GameObject HostPublic;
    public GameObject HostPrivate;
    public GameObject scrollViewMap;


    public override void Initialize()
    {
        HostPublic.SetActive(false);
        HostPrivate.SetActive(false);
    }

    
    public void ActiveButton()
    {
        HostPublic.SetActive(true);
        HostPrivate.SetActive(true);
        scrollViewMap.SetActive(false);
    }


}
