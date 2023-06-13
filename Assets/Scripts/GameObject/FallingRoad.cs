using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRoad : MonoBehaviour
{
    public GameObject[] FallingRoadGameObject;

    public ParticleSystem smokeEffect;
    public GameObject rockEffect;
    public GameObject blackMask;

    private bool isCollised;

    private void ChangeRoadRotation()
    {
        for(int i = 0; i < FallingRoadGameObject.Length; i++)
        {
            int randomValue = Random.Range(0,2);
            int result = randomValue == 0 ? -3 : 3;
            FallingRoadGameObject[i].gameObject.transform.Rotate(0f, 0f, result);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollised)
        {
            isCollised = true;
            ChangeRoadRotation();
            StartCoroutine(PlayerEffect());
        }
    }

    private IEnumerator PlayerEffect()
    {
        yield return new WaitForSeconds(0.25f);
        AudioManager.instance.PlayOneShot("DamagedRoadEndLongMusic");
        blackMask.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        GameObject effect1 = Instantiate(rockEffect, transform.position, Quaternion.identity);
        smokeEffect.Play();
        Destroy(effect1, 3.5f);
    }

    
}
