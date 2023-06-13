using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoor : MonoBehaviour
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
    private GameObject hitRockEffect;

    [SerializeField]
    private ParticleSystem hitSmokeEffect;


    public void PlayHitEffect()
    {
      //  hitSmokeEffect.Play();
        GameObject rockEffect = Instantiate(hitRockEffect, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        GameObject _hitEffect = Instantiate(hitEffect, transform.position + new Vector3(0,-4,0), Quaternion.identity);

        Destroy(rockEffect, 2.0f);
        Destroy(_hitEffect, 2.0f);
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
