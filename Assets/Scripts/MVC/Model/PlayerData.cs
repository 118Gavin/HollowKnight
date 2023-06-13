using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Events;

public class PlayerData
{
    private int health;

    private int coinCount;

    public int cointCount
    {
        get
        {
            return coinCount;
        }

        set
        {
            coinCount = value;
            UpdateCoinInfo();
        }
    }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            UpdateHealthInfo();
        }
    }

    // 加血减血事件
    private event UnityAction addHealthEvent;
    private event UnityAction reduceHealthEvent;
    private event UnityAction<int> updateHealthEvent;
    private event UnityAction<int> updateCoinEvent;

    private static PlayerData instance = null;

    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerData();
            }

            return instance;
        }
    }

    // 初始化数据
    public void Init(int healthNumber, int coinnumber)
    {
        health = healthNumber;
        coinCount = coinnumber;

        UpdateHealthInfo();
        UpdateCoinInfo();
    }

    // 加血
    public void AddHealth()
    {
        health++;

        UpdateAddHealthInfo();
    }

    // 加金币
    public void AddCoin()
    {
        coinCount++;
        UpdateCoinInfo();
    }

    // 减去金币
    public void ReduceCoin()
    {
        coinCount = (int)Mathf.Clamp(coinCount -= 5, 0, Mathf.Infinity);
        UpdateCoinInfo();
    }

    // 扣血
    public void ReduceHealth()
    {
        health--;
        health = Mathf.Clamp(health, 0, 5);
        if (health > 0)
        {
            PlayerController.Instance.Damaged();
        }
        else
        {
            PlayerController.Instance.Dead();
        }


        UpdateReduceHealthInfo();
    }

    // 加血事件
    public void AddHealthEvent(UnityAction func)
    {
        addHealthEvent += func;
    }

    public void RemoveAddHealthEvent(UnityAction func)
    {
        addHealthEvent -= func;
    }

    // 扣血事件
    public void AddReduceHealthEvent(UnityAction func)
    {
        reduceHealthEvent += func;
    }

    public void RemoveReduceHealthEvent(UnityAction func)
    {
        reduceHealthEvent -= func;
    }

    // 更新血量UI事件
    public void AddUpdateHealhtEvent(UnityAction<int> func)
    {
        updateHealthEvent += func;
    }

    public void RemoveUpdateHealthEvent(UnityAction<int> func)
    {
        updateHealthEvent -= func;
    }

    //更新金币数量事件
    public void AddUpdateCoinEvent(UnityAction<int> func)
    {
        updateCoinEvent += func;
    }

    public void RemoveUpdateCoinEvent(UnityAction<int> func)
    {
        updateCoinEvent -= func;
    }



    // 通知外部更新增加血量数据
    public void UpdateAddHealthInfo()
    {
        if (addHealthEvent != null)
        {
            addHealthEvent();
        }
    }

    // 通知外部更新减少血量数据
    public void UpdateReduceHealthInfo()
    {
        if (reduceHealthEvent != null)
        {
            reduceHealthEvent();
        }
    }

    // 通知外部更新当前血量数据
    public void UpdateHealthInfo()
    {
        if (updateHealthEvent != null)
        {
            updateHealthEvent(health);
        }
    }

    // 通知外部更新当前金币数量
    public void UpdateCoinInfo()
    {
        if (updateCoinEvent != null)
        {
            updateCoinEvent(coinCount);
        }
    }

}
