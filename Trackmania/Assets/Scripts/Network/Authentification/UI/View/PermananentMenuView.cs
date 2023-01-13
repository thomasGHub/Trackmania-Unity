using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PermananentMenuView : MonoBehaviour
{
    private static PermananentMenuView _instance;

    public TextMeshProUGUI infosPlayerText;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);

        _instance = this;
    }

    void Start()
    {
        infosPlayerText.text = PlayerPrefs.GetString("UserName");
    }

    public static void ActivateView(bool viewState)
    {
        _instance.gameObject.SetActive(viewState);
    }

}
