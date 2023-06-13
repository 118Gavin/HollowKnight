using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoin : MonoBehaviour
{
    public GameObject coinPrefab;
    public int coinCount;

    [SerializeField]
    private Vector2 coinDirection;

    [SerializeField]
    private float coinSpeed;

    private void Start()
    {
        coinDirection.Normalize();
    }

    public void SpawnCoinFunc()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            int randomValue = Random.Range(0, 2);
            int randomDirection = randomValue == 0 ? 1 : -1;
            GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
            coinRb.velocity = coinDirection * coinSpeed * randomDirection;
        }
    }

}
