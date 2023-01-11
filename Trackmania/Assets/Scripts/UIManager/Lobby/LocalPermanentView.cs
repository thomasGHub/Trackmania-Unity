using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LocalPermanentView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _infoPlayerText;
    [SerializeField] private Button _mainMenu;

    private void Start()
    {
        _infoPlayerText.text = PlayerPrefs.GetString("UserName");
        _mainMenu.onClick.AddListener(MainMenu);
    }

    private void MainMenu()
    {
        Mirror.NetworkManager.singleton.OnDestroySelf();
        SceneManager.LoadScene("Offline");
    }
}
