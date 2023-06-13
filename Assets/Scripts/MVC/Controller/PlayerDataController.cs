using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class PlayerDataController : MonoBehaviour
{
    private PlayerView playerView;
    private void Awake()
    {
        playerView = gameObject.GetComponent<PlayerView>();
        PlayerData.Instance.AddHealthEvent(Addhealth);
        PlayerData.Instance.AddReduceHealthEvent(ReduceHealth);
        PlayerData.Instance.AddUpdateHealhtEvent(UpdateHealth);
        PlayerData.Instance.AddUpdateCoinEvent(UpdateCoinCount);
    }

    private void Addhealth()
    {
        playerView.ShowFullHealthImage();
    }

    private void ReduceHealth()
    {
        playerView.HideFullHealthImage();
    }

    private void UpdateHealth(int health)
    {
        playerView.UpdateHealth(health);
    }

    private void UpdateCoinCount(int coinCount)
    {
        playerView.UpdateCoinCount(coinCount);
    }






}
