using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPermanentView : MonoBehaviour
{
    public static LobbyPermanentView _instance;

    [SerializeField] private TextMeshProUGUI _infoPlayerText;

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);

        _instance = this;
    }
    private void Start()
    {
        _infoPlayerText.text = PlayerPrefs.GetString("UserName");
    }

    public static void ActivateView(bool viewState)
    {
        _instance.gameObject.SetActive(viewState);
    }
}
