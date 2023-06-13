using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float knockBackForce;

    [SerializeField]
    private float forceContinueTimer;

    private Vector2 playerKnockBackDirection;

    private bool isPLayerKnockBack;
    private bool onPlayerLeft; // 判断是否在玩家的左半边

    public bool canAttack;
    public bool isSpike; // 判断是否是地刺

    private void Update()
    {
        HandlePlayerKnockBack();
    }

    // 选择玩家的击退方向
    private void CheckPlayerDirection()
    {
        /*
         * x :1表示向右施加力,-1表示向左施加力
         */
        if (PlayerController.Instance.IsGround)
        {
            isPLayerKnockBack = true;
            if (PlayerController.Instance.IsFackingLeft && onPlayerLeft)
            {
                playerKnockBackDirection.x = 1;
            }
            else if (PlayerController.Instance.IsFackingLeft && !onPlayerLeft)
            {
                PlayerController.Instance.Flip();
                playerKnockBackDirection.x = -1;
            }

            else if (!PlayerController.Instance.IsFackingLeft && onPlayerLeft)
            {
                PlayerController.Instance.Flip();
                playerKnockBackDirection.x = 1;
            }
            else if (!PlayerController.Instance.IsFackingLeft && !onPlayerLeft)
            {
                playerKnockBackDirection.x = -1;
            }

            playerKnockBackDirection.y = 1f;
            playerKnockBackDirection.Normalize();
        }

        else if (!PlayerController.Instance.IsGround)
        {
            isPLayerKnockBack = true;
            if (PlayerController.Instance.IsFackingLeft && onPlayerLeft)
            {
                playerKnockBackDirection.x = 0.5f;
            }
            else if (PlayerController.Instance.IsFackingLeft && !onPlayerLeft)
            {
                PlayerController.Instance.Flip();
                playerKnockBackDirection.x = -0.5f;
            }

            else if (!PlayerController.Instance.IsFackingLeft && onPlayerLeft)
            {
                PlayerController.Instance.Flip();
                playerKnockBackDirection.x = 0.5f;
            }
            else if (!PlayerController.Instance.IsFackingLeft && !onPlayerLeft)
            {
                playerKnockBackDirection.x = -0.5f;
            }

            playerKnockBackDirection.y = 1f;
            playerKnockBackDirection.Normalize();
        }

        StartCoroutine(ForceContinueTimer());

    }

    // 判断敌人在玩家哪边
    private void CheckWhichDirectionFacingPlayer(Vector2 targetPos)
    {
        Vector2 thisPos = gameObject.transform.position;
        Vector2 targetToThis = targetPos - thisPos; // 先计算目标到自身的距离

        float dotProduct = Vector2.Dot(targetToThis, thisPos);

        if (dotProduct > 0)
        {
            onPlayerLeft = true;
        }
        else if (dotProduct < 0)
        {
            onPlayerLeft = false;
        }
        else
        {
            Debug.Log("重叠");
        }
    }

    private void HandlePlayerKnockBack()
    {
        if (isPLayerKnockBack)
        {
            PlayerController.Instance.PlayerRb.velocity = playerKnockBackDirection * knockBackForce;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerController.Instance.CanDamaged && canAttack)
        {
            PlayerData.Instance.ReduceHealth();

            if (isSpike)
            {
                PlayerController.Instance.OnSpike();
            }

            if (!PlayerController.Instance.IsDead)
            {
                Vector2 playerPos = collision.transform.position;
                CheckWhichDirectionFacingPlayer(playerPos);
                CheckPlayerDirection();
            }
        }
    }

    private IEnumerator ForceContinueTimer()
    {
        yield return new WaitForSeconds(forceContinueTimer);
        isPLayerKnockBack = false;
    }
}
