using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFindPlayer : MonoBehaviour
{
    public bool isTouchingPlayer;

    public Transform playerCheck;

    public float playerCheckRadius;

    public LayerMask whatIsPlayer;


    private void Update()
    {
        CheckPlayer();
    }

    private void CheckPlayer()
    {
        isTouchingPlayer = Physics2D.OverlapCircle(playerCheck.position, playerCheckRadius, whatIsPlayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
    }


}
