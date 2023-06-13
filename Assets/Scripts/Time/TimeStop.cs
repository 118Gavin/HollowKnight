using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    private float speed; // 时间增长速度
    private bool isChanging = false; // 判断当前是否正在变化

    public static TimeStop instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            return;
        }
    }

    private void Update()
    {
        if (isChanging)
        {
            if (Time.timeScale < 1)
            {
                isChanging = true;
                Time.timeScale += Time.deltaTime * speed;
            }
            else if (Time.timeScale > 1)
            {
                isChanging = false;
                Time.timeScale = 1;
            }
        }
    }

    public void StopTime(float startTimeScaleValue, float timeSpeed)
    {
        Time.timeScale = startTimeScaleValue;
        speed = timeSpeed;
        isChanging = true;
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void ResetTime()
    {
        Time.timeScale = 1;
    }
}
