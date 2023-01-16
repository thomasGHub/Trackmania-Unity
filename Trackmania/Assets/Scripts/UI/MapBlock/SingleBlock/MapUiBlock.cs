using TMPro;
using UnityEngine;

public class MapUiBlock : MonoBehaviour
{
    [Header("Ui Object")]
    [SerializeField] protected TextMeshProUGUI _textName;
    [SerializeField] protected TextMeshProUGUI _textAuthor;
    [SerializeField] protected TextMeshProUGUI _textDate;

    protected MapInfo _mapInfo;
    
    public virtual void Init(MapInfo mapInfo)
    {
        _mapInfo = mapInfo;

        _textName.text = _mapInfo.Name;
        _textAuthor.text = _mapInfo.Author;
        _textDate.text = _mapInfo.DateTime.ToShortDateString();
    }
}
