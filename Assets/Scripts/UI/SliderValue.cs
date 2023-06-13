using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    public TextMeshProUGUI sliderValue;
    public Slider slider;
    public bool isGlobal;
    public bool isBg;
    public bool isSimple;

 
    private void Start()
    {
        // 绑定更新数据事件
        Debug.Log("绑定");
        AudioManager.instance.UpdateVolumeEvent += OnVolumeUpdate;
        OnVolumeUpdate();
        OnSliderValueChange();
    }

    public void OnSliderValueChange()
    {
        int _value = Mathf.FloorToInt(slider.value);
        sliderValue.text = _value.ToString();

        if (isGlobal)
            SetGlobalVolume();
        else if (isBg)
            SetBgVolume();
        else if (isSimple)
            SetSimpleVolume();
    }

    public void ResettSliderValue()
    {
        slider.value = 10;
        sliderValue.text = 10.ToString();
        SetGlobalVolume();
        SetSimpleVolume();
        SetBgVolume();
    }

    public void SetGlobalVolume()
    {
        AudioManager.instance.SetGlobalVolume(Mathf.FloorToInt(slider.value));
        AudioManager.instance.GlobalVolume = Mathf.FloorToInt(slider.value);
    }

    public void SetBgVolume()
    {
        AudioManager.instance.SetBgVolume(Mathf.FloorToInt(slider.value));
        AudioManager.instance.BgVolume = Mathf.FloorToInt(slider.value);
    }

    public void SetSimpleVolume()
    {
        AudioManager.instance.SetSimpleVolume(Mathf.FloorToInt(slider.value));
        AudioManager.instance.SimpleVolume = Mathf.FloorToInt(slider.value);
    }

    public void OnVolumeUpdate()
    {
        Debug.Log("执行");
        if (isGlobal)
        {
            slider.value = AudioManager.instance.GlobalVolume;
            sliderValue.text = AudioManager.instance.GlobalVolume.ToString();
        }
        else if (isBg)
        {
            slider.value = AudioManager.instance.BgVolume;
            sliderValue.text = AudioManager.instance.BgVolume.ToString();
        }
        else if (isSimple)
        {
            slider.value = AudioManager.instance.SimpleVolume;
            sliderValue.text = AudioManager.instance.SimpleVolume.ToString();
        }
    }
}
