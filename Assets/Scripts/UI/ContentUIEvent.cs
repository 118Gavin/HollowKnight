using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContentUIEvent : MonoBehaviour
{
    public Image blackMask;

    public GameObject leftImage;
    public GameObject rightImage;

    // 鼠标经过
    public void OnMousePointEnter()
    {
        AudioManager.instance.PlayOneShot("ButtonEnterMusic");
        leftImage.SetActive(true);
        rightImage.SetActive(true);
    }

    // 鼠标离开
    public void OnMousePointExit()
    {
        leftImage.SetActive(false);
        rightImage.SetActive(false);
    }


    // 单击退出
    public void OnMouseExitClick()
    {
        AudioManager.instance.PlayOneShot("ButtonClickMusic");
        Application.Quit();
    }

    // 显示内容
    public void DisplayerContent()
    {
        AudioManager.instance.PlayOneShot("ButtonClickMusic");
        gameObject.SetActive(true);
    }

    // 隐藏内容
    public void HideContent()
    {
        AudioManager.instance.PlayOneShot("ButtonClickMusic");
        gameObject.SetActive(false);
    }

    // 返回主菜单
    public void ReturnMainMenu()
    {
        AudioManager.instance.PlayOneShot("ButtonClickMusic");
        TimeStop.instance.ResetTime();  
        StartCoroutine(OnChangeScence());
    }

    // 当玩家返回之后在保存玩家的声音设置
    public void SaveVolumeToLocal()
    {
        AudioManager.instance.SaveVolumeData();
    }

    public void PauseReturnToGame()
    {
        AudioManager.instance.PlayOneShot("ButtonClickMusic");
        gameObject.SetActive(false);
        TimeStop.instance.ResetTime();
    }

    private IEnumerator OnChangeScence()
    {
        blackMask.DOFade(1f, 1f);
        yield return new WaitForSeconds(2.0f);
        blackMask.DOFade(0f, 1f);
        PlayerController.Instance.gameObject.SetActive(false);
        SceneManager.LoadScene("StartScene");
    }
}
