using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Text;

public class CampaignButtonUI : MonoBehaviour, IPointerEnterHandler
{
    public int buttonNumber;
    public TextMeshProUGUI numberText;
    public Image imageMedal;
    public TextMeshProUGUI top;
    public TextMeshProUGUI rankText;


    private void Start()
    {
        imageMedal.gameObject.SetActive(true);
        rankText.gameObject.SetActive(false);
        top.gameObject.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        SoloCampaignMenuView.instance.MouseOnButtonNumber(buttonNumber);
    }


    public void ApplyRankButton(Image _image)
    {
        imageMedal = _image;
    }


    public void CategoryChange(bool isWorld)
    {
        if (isWorld)
        {
            imageMedal.gameObject.SetActive(false);
            rankText.gameObject.SetActive(true);
            top.gameObject.SetActive(true);
        }
        else
        {
            imageMedal.gameObject.SetActive(true);
            rankText.gameObject.SetActive(false);
            top.gameObject.SetActive(false);
        }
    }


    


}
