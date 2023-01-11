using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPermanentView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoPlayerText;

    private void Start()
    {
        _infoPlayerText.text = PlayerPrefs.GetString("UserName");
    }
}
