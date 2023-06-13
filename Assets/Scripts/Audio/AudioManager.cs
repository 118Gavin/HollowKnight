using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private JsonWriteReadSystem data;

    private int globalVolume;
    public int GlobalVolume
    {
        set
        {
            globalVolume = value;
        }

        get
        {
            return globalVolume;
        }
    }

    private int bgVolume;
    public int BgVolume
    {
        set
        {
            bgVolume = value;
        }
        get
        {
            return bgVolume;
        }
    }

    private int simpleVolume;
    public int SimpleVolume
    {
        set
        {
            simpleVolume = value;
        }

        get
        {
            return simpleVolume;
        }
    }

    public event UnityAction UpdateVolumeEvent;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }

        SceneManager.sceneLoaded += OnScenceLoad;
    }

    private void Start()
    {

        data = GetComponent<JsonWriteReadSystem>();

        data.LoadVolumeToData();
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, item => item.audioName == name);
        if (s == null)
            return;

        // 判断是否正在播放,不然一直从头播放
        if (!s.audioSource.isPlaying)
            s.audioSource.Play();
    }

    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, item => item.audioName == name);
        if (s == null)
            return;
        s.audioSource.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, item => item.audioName == name);
        if (s == null)
            return;

        s.audioSource.Stop();
    }

    public void SetGlobalVolume(int value)
    {
        for (int i = 0; i < sounds.Length - 1; i++)
        {
            sounds[i].audioSource.volume = value * 0.1f;
        }
    }

    public void SetBgVolume(int value)
    {
        for (int i = 0; i < sounds.Length - 1; i++)
        {
            if (sounds[i].isBGM)
                sounds[i].audioSource.volume = value * 0.1f;
        }
    }

    public void SetSimpleVolume(int value)
    {
        for (int i = 0; i < sounds.Length - 1; i++)
        {
            if (!sounds[i].isBGM)
                sounds[i].audioSource.volume = value * 0.1f;
        }
    }


    public void OnScenceLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            Play("TitleMusic");
            Stop("SquareMusic");
        }
        else if (scene.name == "GameScene")
        {
            Play("SquareMusic");
            Stop("TitleMusic");
        }
    }

    public void SaveVolumeData()
    {
        data.SaveVolumeToJson();
    }

    public void LoadVolumeData()
    {
        data.LoadVolumeToData();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnScenceLoad;
    }

   

}
