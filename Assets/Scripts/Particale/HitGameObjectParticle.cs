using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGameObjectParticle : MonoBehaviour
{
    public void FinishAnim()
    {
        Destroy(gameObject);
    }
}
