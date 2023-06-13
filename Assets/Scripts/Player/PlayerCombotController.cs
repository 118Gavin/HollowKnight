using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombotController : MonoBehaviour
{
    private Animator playerAnimator;

    [SerializeField]
    private bool canAttack;

    [SerializeField]
    private float combotMaxTimer;

    [SerializeField]
    private float combotWaitTimer;

    [SerializeField]
    private float upAndDownWaitTimer;

    private bool gotInput;
    private bool isFirstAttack;
    private float lastAttackTime;

    private bool isAttacking;

    private void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }


    private void Update()
    {

        CheckAttackInput();
        CheckCombot();
        WhenDamagedResetAttack();

    }

    // 当玩家攻击时被攻击直接结束攻击状态
    private void WhenDamagedResetAttack()
    {
        if (PlayerController.Instance.IsDamaged)
        {
            PlayerController.Instance.EnableFilpAnimEvent();
            isAttacking = false;
            playerAnimator.SetBool("IsAttacking", isAttacking);
        }

        if (PlayerController.Instance.IsDead)
        {
            isAttacking = false;
            playerAnimator.SetBool("IsAttacking", isAttacking);
        }
    }



    private void CheckAttackInput()
    {
        if (PlayerController.Instance.CanActive)
        {
            // 上+X
            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.X))
            {
                if (canAttack && !isAttacking)
                {
                    isAttacking = true;
                    AudioManager.instance.Play("UpAndDownAttackMusic");
                    playerAnimator.SetBool("IsAttacking", isAttacking);
                    playerAnimator.SetTrigger("IsUpAttack");
                }
            }
            // 在地面不能下劈,下+x
            else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.X) && !PlayerController.Instance.IsGround)
            {
                if (canAttack && !isAttacking)
                {
                    isAttacking = true;
                    AudioManager.instance.Play("UpAndDownAttackMusic");
                    playerAnimator.SetBool("IsAttacking", isAttacking);
                    playerAnimator.SetTrigger("IsDownAttack");
                }
            }

            //普通攻击
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (canAttack)
                {
                    gotInput = true;
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    private void CheckCombot()
    {
        if (Time.time > lastAttackTime + combotMaxTimer || !canAttack)
        {
            gotInput = false;
            isFirstAttack = false;
        }

        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                if (isFirstAttack)
                {
                    AudioManager.instance.Play("FirstAttackMusic");
                    playerAnimator.SetTrigger("FirstAttack");
                }
                else
                {
                    AudioManager.instance.Play("SecondAttackMusic");
                    playerAnimator.SetTrigger("SecondAttack");
                }

                playerAnimator.SetBool("IsAttacking", isAttacking);
                playerAnimator.SetBool("Attack1", true);
            }
        }
    }

    public void FinishFirstAttack()
    {
        isAttacking = false;
        playerAnimator.SetBool("IsAttacking", isAttacking);
        playerAnimator.SetBool("Attack1", false);
    }

    // 第二次攻击需要停顿0.25f
    public void FinishSecondAttack()
    {
        isAttacking = false;
        playerAnimator.SetBool("IsAttacking", isAttacking);
        playerAnimator.SetBool("Attack1", false);
        StartCoroutine(CombotAttackWaitTimer());
    }

    public void FinishUpAndDownAttack()
    {
        isAttacking = false;
        playerAnimator.SetBool("IsAttacking", isAttacking);
        StartCoroutine(UpAndDownAttackWaitTimer());
    }

    private IEnumerator CombotAttackWaitTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(combotWaitTimer);
        canAttack = true;
    }

    private IEnumerator UpAndDownAttackWaitTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(upAndDownWaitTimer);
        canAttack = true;
    }


}
