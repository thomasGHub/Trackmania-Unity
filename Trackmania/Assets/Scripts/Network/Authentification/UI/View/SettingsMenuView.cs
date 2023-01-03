using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenuView : View
{
    public Slider sliderMusic;
    public Slider sliderFx;
    public Button BackButton;

    public override void Initialize()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.volumeMusicSlider = sliderMusic;
            AudioManager.instance.volumeFXSlider = sliderFx;
            AudioManager.instance.InitializeSliders();
        }


        BackButton.onClick.AddListener(() => ViewManager.ShowLast());
            
    }

}
