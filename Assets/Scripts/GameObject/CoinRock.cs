using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRock : MonoBehaviour
{
    [SerializeField]
    private float shakeMagnitude;

    [SerializeField]
    private int health;

    [SerializeField]
    private float shakeTimerSet;

    public GameObject hitEnemyEffect;
    public Sprite gameObjectDeadImage;

    private SpriteRenderer spriteRenderer;
    private SpawnCoin spawnCoin;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isDead;
    private float shakeTimer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnCoin = GetComponent<SpawnCoin>();
        // 记录物体的初始位置和旋转
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // 在一段时间内随机改变物体的位置或旋转
            transform.position = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            transform.rotation = new Quaternion(
                initialRotation.x + Random.Range(-shakeMagnitude, shakeMagnitude) * 0.2f,
                initialRotation.y + Random.Range(-shakeMagnitude, shakeMagnitude) * 0.2f,
                initialRotation.z + Random.Range(-shakeMagnitude, shakeMagnitude) * 0.2f,
                initialRotation.w + Random.Range(-shakeMagnitude, shakeMagnitude) * 0.2f
            );

            shakeTimer -= Time.deltaTime;
        }
    }


    private void PlayHitEnemyParticale()
    {
        if (PlayerController.Instance.IsFackingLeft)
        {
            Instantiate(hitEnemyEffect, gameObject.transform.position, Quaternion.Euler(0.0f, -180f, Random.Range(-10.0f, 30f)));
        }
        else
        {
            Instantiate(hitEnemyEffect, gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(-10.0f, 25f)));

        }
        AudioManager.instance.PlayOneShot("DamagedRoadEndSmallMusic");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SwordController>() != null && !isDead)
        {
            health--;
            if (health <= 0)
            {
                AudioManager.instance.PlayOneShot("DamagedRoadEndMusic");
                isDead = true;
                spriteRenderer.sprite = gameObjectDeadImage;
                return;
            }

            shakeTimer = shakeTimerSet;
            spawnCoin.SpawnCoinFunc();
            PlayHitEnemyParticale();
        }
    }
}
