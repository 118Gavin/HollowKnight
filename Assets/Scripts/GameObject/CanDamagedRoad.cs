using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDamagedRoad : MonoBehaviour
{
    [SerializeField]
    private int health;

    public int Health
    {
        get
        {
            return health;
        }
    }

    [SerializeField]
    private GameObject hitEffect;

    [SerializeField]
    private GameObject hitWoodEffect;

    [SerializeField]
    private GameObject hitRockEffect;

    [SerializeField]
    private ParticleSystem hitSmokeEffect;

    [SerializeField]
    private GameObject endRockEffect;

    [SerializeField]
    private GameObject endWoodEffect;

    public void PlayHitEffect()
    {
        hitSmokeEffect.Play();
        GameObject woodEffect = Instantiate(hitWoodEffect, transform.position, Quaternion.identity);
        GameObject rockEffect = Instantiate(hitRockEffect, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        GameObject _hitEffect = Instantiate(hitEffect, transform.position, Quaternion.identity);

        Destroy(woodEffect, 2.0f);
        Destroy(rockEffect, 2.0f);
        Destroy(_hitEffect, 2.0f);
    }

    public void PlayerDestoryEffect()
    {
        hitSmokeEffect.Play();
        GameObject woodEffect = Instantiate(endWoodEffect, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        GameObject rockEffect = Instantiate(endRockEffect, transform.position + new Vector3(0, 2, 0), Quaternion.identity);

        Destroy(woodEffect, 2.0f);
        Destroy(rockEffect, 2.0f);
    }

    public void reduceHealth()
    {
        health--;
    }

    public void onDead()
    {
        gameObject.SetActive(false);
    }
}
