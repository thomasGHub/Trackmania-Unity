using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPermanentView : MonoBehaviour
{
    public static LobbyPermanentView instance;

    [SerializeField] private TextMeshProUGUI _infoPlayerText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _infoPlayerText.text = PlayerPrefs.GetString("UserName");
    }
}
