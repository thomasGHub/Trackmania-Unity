using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermananentMenuView : MonoBehaviour
{
    public TextMeshProUGUI infosPlayerText;
    void Start()
    {
        infosPlayerText.text = PlayerPrefs.GetString("UserName");
    }

}
