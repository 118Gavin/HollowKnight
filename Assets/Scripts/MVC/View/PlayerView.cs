using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public GameObject[] fullHealthImages;
    public TextMeshProUGUI coinText;

    // 扣血隐藏指定的血条图片
    public void HideFullHealthImage()
    {
        for (int i = fullHealthImages.Length - 1; i >= 0; i--)
        {
            // 从后往前开始判断如果遇到第一个显示的血条图片隐藏
            if (fullHealthImages[i].activeInHierarchy == true)
            {
                fullHealthImages[i].SetActive(false);
                break;
            }
        }
    }

    public void ShowFullHealthImage()
    {
        // 从前往后判断遇到第一个隐藏的图片显示
        foreach (var image in fullHealthImages)
        {
            if (image.activeInHierarchy == false)
            {
                image.SetActive(true);
                break;
            }
        }
    }

    public void UpdateHealth(int health)
    {
        foreach (var image in fullHealthImages)
        {
            if (health > 0)
                image.SetActive(true);
            else
                image.SetActive(false);

            health--;
        }
    }

    public void UpdateCoinCount(int coinCount)
    {
        coinText.text = coinCount.ToString();
    }

}
