using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField]
    private int damage;

    [SerializeField]
    private float setForceContinueTime; // 后坐力的持续时间

    [SerializeField]
    private float playerKnockBackForce; // 玩家被击退力

    [SerializeField]
    private float upWardForce; // 向上的后坐力力

    private EnemyController enemyController;
    private Vector2 playerDirection; // 保存玩家武器攻击时的朝向
    private Vector2 enemyDirection; //保存敌人力的朝向
    private bool isCollised; // 判断是否执行后坐力
    private bool falling;
    private float forceContinueTimer;

    private void Update()
    {
        if (forceContinueTimer > 0)
        {
            forceContinueTimer -= Time.deltaTime;
        }
        else
        {
            if (enemyController != null)
            {
                enemyController.IsEnemyKnockBack = false;
                enemyController = null;
                isCollised = false;
                falling = false;
            }
        }
    }

    private void FixedUpdate()
    {
        HandlePlayerMovement();
        HandleEnemyMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollised)
        {
            // 如果是敌人则执行
            if (collision.GetComponent<EnemyController>() != null)
            {
                enemyController = collision.GetComponent<EnemyController>();
                forceContinueTimer = setForceContinueTime;
                if (!enemyController.IsDead)
                    CheckGameObject();
            }

            if (collision.GetComponent<AttackGrassAndRock>() != null)
            {
                AttackGrassAndRock grassAndRock = collision.GetComponent<AttackGrassAndRock>();
                grassAndRock.PlayEffect();
                return;
            }

            if (collision.GetComponent<FallingGameObject>() != null)
            {
                FallingGameObject fallingGameObject = collision.GetComponent<FallingGameObject>();
                fallingGameObject.OnAttack();
            }

            if (collision.GetComponent<CanDamagedRoad>() != null)
            {
                CanDamagedRoad canDamagedRoad = collision.GetComponent<CanDamagedRoad>();

                if (canDamagedRoad.Health > 0)
                {
                    AudioManager.instance.PlayOneShot("DamagedRoadEndLongMusic");
                    canDamagedRoad.PlayHitEffect();
                    canDamagedRoad.reduceHealth();
                }
                else
                {
                    AudioManager.instance.PlayOneShot("DamagedRoadEndMusic");
                    canDamagedRoad.PlayerDestoryEffect();
                    canDamagedRoad.onDead();
                }
            }

            if(collision.GetComponent<BigDoor>() != null)
            {
                BigDoor bigDoor = collision.GetComponent<BigDoor>();
                if(bigDoor.Health > 0)
                {
                    AudioManager.instance.PlayOneShot("DamagedRoadEndLongMusic");
                    bigDoor.PlayHitEffect();
                    bigDoor.reduceHealth();
                }
                else
                {
                    Debug.Log("通过");
                    AudioManager.instance.PlayOneShot("DamagedRoadEndMusic");
                    bigDoor.PlayHitEffect();
                    bigDoor.onDead();
                    
                }
            }
        }
    }


    // 检查物体
    private void CheckGameObject()
    {
        /*
         * 玩家朝向判定
         */

        // 如果玩家左右攻击并且玩家在地面上,则可以有左右的后坐力
        if ((Input.GetAxisRaw("Horizontal") != 0 && PlayerController.Instance.IsGround) || Input.GetAxisRaw("Horizontal") == 0)
        {
            isCollised = true;

            if (PlayerController.Instance.IsFackingLeft)
            {
                playerDirection = Vector2.right;
                enemyDirection = Vector2.left;
            }
            else
            {
                playerDirection = Vector2.left;
                enemyDirection = Vector2.right;
            }
        }

        // 如果物体支持向上的力,玩家向下砍不在地面,则可以有向上的力
        if (enemyController.giveUpForce && Input.GetAxisRaw("Vertical") == -1 && !PlayerController.Instance.IsGround)
        {
            falling = true;
            isCollised = true;
            playerDirection = Vector2.up;
        }

        // 如果玩家向上砍刀物体不在地面,则获得向下的力
        if (Input.GetAxisRaw("Vertical") == 1 && !PlayerController.Instance.IsGround)
        {
            playerDirection = Vector2.down;
            isCollised = true;
        }

        enemyController.TakeDamage(damage);
    }

    // 玩家攻击不同物体,会得到不同力的反馈
    private void HandlePlayerMovement()
    {
        // 如果碰撞
        if (isCollised)
        {
            if (falling)
            {
                PlayerController.Instance.PlayerRb.velocity = playerDirection * upWardForce;
            }
            else
            {
                PlayerController.Instance.PlayerRb.velocity = playerDirection * playerKnockBackForce;
            }
        }
    }

    // 玩家攻击不同物体,会得到不同力的反馈
    private void HandleEnemyMovement()
    {
        // 如果碰撞
        if (isCollised && enemyController.CanKnockBack)
        {
            enemyController.EnemyKnockBack(enemyDirection);
        }
    }


}
