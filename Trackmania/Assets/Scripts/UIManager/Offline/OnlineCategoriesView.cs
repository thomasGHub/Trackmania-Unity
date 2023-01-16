using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineCategoriesView : View
{
    public Button SoloButton;
    public Button OnlineButton;
    public Button LanButton;
    public Button BackButton;
    public override void Initialize()
    {
        SoloButton.onClick.AddListener(() => OnSoloClicked()) ;
        OnlineButton.onClick.AddListener(() => OnOnlineClicked());
        LanButton.onClick.AddListener(() => OnLanClicked());
        BackButton.onClick.AddListener(() => OnBack());

        OfflineMode();
    }

    private void OfflineMode()
    {
        if (PlayerPrefs.HasKey("PlayOffline") && PlayerPrefs.GetString("PlayOffline") == "true")
        {
            OnlineButton.interactable = false;
            LanButton.interactable = false;
        }
    }

    void OnHide()
    {
        ViewManager.GetView<SoloMenuView>().Hide();
        ViewManager.GetView<OnlineMenuView>().Hide();
        ViewManager.GetView<LanMenuView>().Hide();
    }

    void OnSoloClicked()
    {
        OnHide();
        ViewManager.Show<SoloMenuView>(remember: true, hideOther: false);
    }

    void OnOnlineClicked()
    {
        OnHide();
        ViewManager.Show<OnlineMenuView>(remember: true, hideOther: false);
    }
    void OnLanClicked()
    {
        OnHide();
        ViewManager.Show<LanMenuView>(remember: true, hideOther: false);
    }

    void OnBack()
    {
        ViewManager.Show<MainMenuView>();
    }
}
