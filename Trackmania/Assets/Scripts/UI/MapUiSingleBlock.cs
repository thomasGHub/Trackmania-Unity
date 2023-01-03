using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapUiSingleBlock : MonoBehaviour
{
    [Header("Ui Object")]
    [SerializeField] private TextMeshProUGUI _textName;
    [SerializeField] private TextMeshProUGUI _textAuthor;
    [SerializeField] private TextMeshProUGUI _textDate;
    [SerializeField] private Button _button;

    private MapInfo _mapInfo;
    
    public void Init(MapInfo mapInfo)
    {
        _mapInfo = mapInfo;

        _textName.text = _mapInfo.Name;
        _textAuthor.text = _mapInfo.Author;
        _textDate.text = _mapInfo.DateTime.ToShortDateString();

        _button.onClick.AddListener(EditMap);
    }

    private void EditMap()
    {
        Debug.Log("MapName : " + _mapInfo.Name);
    }
}
