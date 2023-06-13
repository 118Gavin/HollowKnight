using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnPlayerEnterPos : MonoBehaviour
{
    public int PosCount;
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boxCollider2D.enabled = false;
            CharacterResetPosition.Instance.EnterPoint(PosCount);
        }
    }
}
