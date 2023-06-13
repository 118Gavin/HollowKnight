using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public GameObject CoinGetEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(CoinGetEffect, collision.gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(-30f, 30f)));
            AudioManager.instance.Play("GetCoinMusic");
            PlayerData.Instance.AddCoin();
            GameObject parentObject = gameObject.transform.parent.gameObject;
            Destroy(parentObject);
        }
    }


}
