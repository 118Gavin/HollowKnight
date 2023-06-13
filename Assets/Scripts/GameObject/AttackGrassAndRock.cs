using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGrassAndRock : MonoBehaviour
{
    public GameObject particleEffect;

    public GameObject hitEffect;

    public Sprite gameObjectDeadImage;

    private SpriteRenderer spriteRenderer;

    private BoxCollider2D[] boxCollider2D; // 用于阻挡玩家

    public bool isGrass;
    public bool isRock;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponentsInChildren<BoxCollider2D>();
    }

    public void PlayEffect()
    {
        spriteRenderer.sprite = gameObjectDeadImage;

        foreach (BoxCollider2D collider in boxCollider2D)
        {
      
                collider.enabled = false;
        }

        // 播放玩家攻击特效
        if (PlayerController.Instance.IsFackingLeft)
        {
            Instantiate(hitEffect, gameObject.transform.position, Quaternion.Euler(0.0f, -180f, Random.Range(-10.0f, 30f)));
        }
        else
        {
            Instantiate(hitEffect, gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(-10.0f, 25f)));

        }

        // 判断是草还是石头播放不同的音效
        if (isGrass)
        {
            int randomInt = Random.Range(0, 2);
            if (randomInt == 0)
                AudioManager.instance.PlayOneShot("GrassCutMusic1");
            else
            {
                AudioManager.instance.PlayOneShot("GrassCutMusic2");
            }
        }
        else if (isRock)
        {
            int randomInt = Random.Range(0, 1);
            if (randomInt == 0)
                AudioManager.instance.PlayOneShot("RockHitMusic1");
            else
            {
                AudioManager.instance.PlayOneShot("RockHitMusic2");
            }
        }

        if (isGrass)
        {
            GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);
            Destroy(effect, 1.0f);
        }

        else if (isRock)
        {
            if (PlayerController.Instance.IsFackingLeft)
            {
                GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                Destroy(effect, 3.5f);
            }
            else
            {
                GameObject effect = Instantiate(particleEffect, transform.position, Quaternion.identity);
                Destroy(effect, 3.5f);
            }
        }



    }

    //public void ResetGameObject()
    //{
    //    gameObjectDead.SetActive(false);
    //    boxCollider2D.enabled = true;
    //    this.gameObject.SetActive(true);
    //}
}
