using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuView : View
{
    public Slider sliderMusic;
    public Slider sliderFx;

    public override void Initialize()
    {
        AudioManager.instance.volumeMusicSlider = sliderMusic;
        AudioManager.instance.volumeFXSlider = sliderFx;
    }
}
