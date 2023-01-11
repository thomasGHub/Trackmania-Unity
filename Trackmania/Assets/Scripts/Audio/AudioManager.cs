using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Range(0f, 1f)]
    public float MusicVolume = 1f;

    [Range(0f, 1f)]
    public float FXVolume = 1f;

    [Space]
    public Slider volumeMusicSlider;

    [Space]
    public Slider volumeFXSlider;

    [Space]
    public Sound[] sounds;


    void Awake()
    {
        if (PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("FXVolume"))
        {
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            FXVolume = PlayerPrefs.GetFloat("FXVolume");
        }


        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.soundType == SoundType.Music ? s.volume * MusicVolume : s.volume * FXVolume;

            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }
        InitializeSliders();
    }

    public void InitializeSliders()
    {
        if (volumeMusicSlider!= null && volumeFXSlider!=null)
        {
            volumeMusicSlider.value = MusicVolume;
            volumeMusicSlider.onValueChanged.AddListener(delegate { ValueChangeSliderCheck(); });

            volumeFXSlider.value = FXVolume;
            volumeFXSlider.onValueChanged.AddListener(delegate { ValueChangeSliderCheck(); });
        }
        
    }

    public void Start()
    {

        Play("Theme");

    }

    public void ValueChangeSliderCheck()
    {
        MusicVolume = volumeMusicSlider.value;

        FXVolume = volumeFXSlider.value;

        foreach (Sound s in sounds)
        {
            if (s.source == null)
                return;
            s.source.volume = s.soundType == SoundType.Music ? s.volume * MusicVolume : s.volume * FXVolume;
            s.source.pitch = s.pitch;
        }
    }

    private void OnValidate() //lorsque la valleur change
    {
        foreach (Sound s in sounds)
        {
            if (s.source == null)
                return;
            s.source.volume = s.soundType == SoundType.Music ? s.volume * MusicVolume : s.volume * FXVolume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }



    public void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("FXVolume", FXVolume);
    }

}