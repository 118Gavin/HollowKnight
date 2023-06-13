using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject hitGameObjectParticle;

    [SerializeField]
    private GameObject hitEnemyParticle;

    [SerializeField]
    private GameObject hitDownAttackParticle;

    [SerializeField]
    private int Enemyhealth;

    [SerializeField]
    private float enemyKnockBackForce;

    [SerializeField]
    private float playerKnockBackForce;

    [SerializeField]
    private bool isEnemy; // 是否是敌人

    [SerializeField]
    private bool isWall; // 是否是墙

    [SerializeField]
    private bool isGroundThorn; // 是否是地刺

    [SerializeField]
    private bool canKnockBack;

    public bool CanKnockBack
    {
        get
        {
            return canKnockBack;
        }
    }

    private bool isEnemyKnockBack; // 判断是否在击退状态

    public bool IsEnemyKnockBack
    {
        set
        {
            isEnemyKnockBack = value;
        }

        get
        {
            return isEnemyKnockBack;
        }
    }

    private bool isFacingRight;
    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }

        set
        {
            isFacingRight = value;
        }
    }

    [SerializeField]
    private float destoryTimer; // 销毁时间

    [SerializeField]
    private float fadeOutTimer; // 淡出时间

    [SerializeField]
    private float underAttackTime; // 每次受攻击的间隔

    [SerializeField]
    private float forceContineTime; // 击退玩家力的时间


    public bool giveUpForce; // 用于判断是否玩家攻击时是否给向上的力

    private int currentHealth;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    private Rigidbody2D enemyRb;
    private Animator enemyAnim;
    private EnemyAttack enemyAttack;
    private SpawnCoin spawnCoin;

    private bool hit; // 判定玩家一次攻击一次,防止一次多次判定的bug

    private bool isDead;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }


    private void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        spawnCoin = GetComponent<SpawnCoin>();
        currentHealth = Enemyhealth;
    }

    public void TakeDamage(int damage)
    {
        // 墙壁
        if (isWall)
        {
            AudioManager.instance.Play("HitWallMusic");
            PlayerHitGameObjectParticale();
        }

        // 地刺
        if (isGroundThorn)
        {
            PlayerDownAttackHitGameObject();
        }

        // 其他是场景中的敌人
        else if (isEnemy && !hit && currentHealth >= 0)
        {
            AudioManager.instance.Play("EnemyDamageMusic");
            PlayerHitEnemyParticale();
            currentHealth -= damage;
            hit = true;
            if (currentHealth <= 0)
            {
                StartCoroutine(OnDead());
            }
            else
            {
                StartCoroutine(TurnOfHit());
            }
        }
    }

    private void PlayerHitGameObjectParticale()
    {
        // 复制一份玩家攻击特效
        Instantiate(hitGameObjectParticle, gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
    }

    private void PlayerHitEnemyParticale()
    {
        if (PlayerController.Instance.IsFackingLeft)
        {
            Instantiate(hitEnemyParticle, gameObject.transform.position, Quaternion.Euler(0.0f, -180f, Random.Range(-10.0f, 30f)));
        }
        else
        {
            Instantiate(hitEnemyParticle, gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(-10.0f, 25f)));

        }
    }

    private void PlayerDownAttackHitGameObject()
    {
        Vector3 particlePos = gameObject.transform.position + new Vector3(0, 0.5f, 0);
        Instantiate(hitDownAttackParticle, particlePos, Quaternion.Euler(Vector3.zero));

    }

    public void EnemyKnockBack(Vector2 direction)
    {
        isEnemyKnockBack = true;
        if (enemyRb.gravityScale == 0)
        {
            Vector3 targetPosition = transform.position + new Vector3(direction.x * enemyKnockBackForce, direction.y * enemyKnockBackForce, 0);
            float smoothSpeed = 0.5f; // 调整平滑移动的速度

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
        else
            enemyRb.velocity = direction * enemyKnockBackForce;
    }


    // 0.2秒之后才能再次受到攻击
    private IEnumerator TurnOfHit()
    {
        yield return new WaitForSeconds(underAttackTime);
        hit = false;
    }


    private IEnumerator OnDead()
    {
        spawnCoin.SpawnCoinFunc();
        enemyAttack.canAttack = false;
        isDead = true;
        enemyAnim.SetTrigger("Dead");
        AudioManager.instance.Play("EnemyDeadMusic");
        yield return new WaitForSeconds(destoryTimer);

    }

}
