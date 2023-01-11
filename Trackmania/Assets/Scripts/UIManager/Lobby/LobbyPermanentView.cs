using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPermanentView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoPlayerText;
    [SerializeField] private Button _leaveNetworkButton;

    private void Start()
    {
        _infoPlayerText.text = PlayerPrefs.GetString("UserName");

        _leaveNetworkButton.onClick.AddListener(HideNetworkButton);
        _leaveNetworkButton.gameObject.SetActive(false);
    }

    private void HideNetworkButton()
    {
        _leaveNetworkButton.gameObject.SetActive(false);
    }
}
