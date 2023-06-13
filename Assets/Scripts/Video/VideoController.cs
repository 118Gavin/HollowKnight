using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [SerializeField]
    private VideoClip[] videoClips;

    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    public void FirstPlay()
    {
        AudioManager.instance.Stop("TitleMusic");
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name != "Video" && gameObject.name != "Main Camera" && gameObject.name != "AudioManager")
            {
                gameObject.SetActive(false);
            }
        }

        videoPlayer.clip = videoClips[0];
        videoPlayer.loopPointReached += OnViedoEnd;
        videoPlayer.Play();
    }

    private void OnViedoEnd(VideoPlayer vb)
    {
        videoPlayer.clip = videoClips[1];
        videoPlayer.loopPointReached += OnLastViedoEnd;
        videoPlayer.Play();
    }

    private void OnLastViedoEnd(VideoPlayer vb)
    {
        videoPlayer.Stop();
        GameObject _gameObject = GameObject.Find("Player");
        if (_gameObject != null)
            PlayerController.Instance.gameObject.SetActive(true);
        SceneManager.LoadScene("GameScene");
    }
}
