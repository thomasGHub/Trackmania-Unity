using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Music,
    FX
}


[System.Serializable]
public class Sound
{
    public string name = string.Empty;
    public SoundType soundType = SoundType.FX;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;

    //[HideInInspector]
    public AudioSource source = null;
}