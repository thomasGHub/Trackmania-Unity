using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuView : View
{
    public Button PlayButton;
    public Button CreateButton;
    public Button MarketPLaceButton;
    public Button SettingsButton;

    public override void Initialize()
    {
        PlayButton.onClick.AddListener(() => PlayClicked());
        CreateButton.onClick.AddListener(() => CreateClicked());
        MarketPLaceButton.onClick.AddListener(() => MarketPlaceClicked());
        SettingsButton.onClick.AddListener(() => SettingsClicked());
    }

    void PlayClicked()
    {
        ViewManager.Show<OnlineCategoriesView>();

    }

    void CreateClicked()
    {
        //Load scene
    }

    void MarketPlaceClicked()
    {

    }

    void SettingsClicked()
    {
        ViewManager.Show<SettingsMenuView>();
    }
}
