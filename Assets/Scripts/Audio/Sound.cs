using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip audioClip;

    public string audioName;

    [Range(0, 1)]
    public float volume;

    [Range(0, 1)]
    public float pitch;

    public bool loop;

    public bool isBGM;

    [HideInInspector]
    public AudioSource audioSource;
}
