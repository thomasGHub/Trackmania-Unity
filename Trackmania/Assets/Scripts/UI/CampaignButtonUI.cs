using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CampaignButtonUI : MonoBehaviour, IPointerEnterHandler
{
    public int buttonNumber;
    public Image imageMedal;
    public TextMeshProUGUI top;
    public TextMeshProUGUI textScore;


    private void Start()
    {
        imageMedal.gameObject.SetActive(true);
        textScore.gameObject.SetActive(false);
        top.gameObject.SetActive(false);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        SoloCampaignMenuView.instance.MouseOnButtonNumber(buttonNumber);
    }

    public void ApplyRankButton(string _score)
    {
        textScore.text = _score;
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
            textScore.gameObject.SetActive(true);
            top.gameObject.SetActive(true);
        }
        else
        {
            imageMedal.gameObject.SetActive(true);
            textScore.gameObject.SetActive(false);
            top.gameObject.SetActive(false);
        }
    }


}
