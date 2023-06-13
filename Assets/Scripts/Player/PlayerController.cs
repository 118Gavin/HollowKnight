using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public Animator wingEffectAnim;
    public GameObject doubleJumpParticle;
    public GameObject pauseMenu;
    public GameObject damagedEffect;

    public float walkSpeed;
    public float moveSpeedInAir;
    public float jumpSpeed; // 普通跳跃力
    public float doubleJumpSpeed; // 二段跳跃力
    public float jumpFallingMult; // 下降的速率
    public float jumpTimerSet; // 在角色落地前,设置一个时间的判断玩家是否再次按下跳跃键
    public float turnTimerSet; // 在贴墙跳转身前,设置一个时间来判定用户移动的方向
    public float jumpLongTimeThreshold; // 长按跳跃键的时间
    public int amoutJumpCount; // 最大跳跃次数
    public float groundCheckRadius; // 检查地面的半径
    public float wallCheckDistance; // 检查墙的直径
    public float LandSpeed; // 满足着陆动画的速度
    public float defaultGravityScale; // 默认的重力大小
    public float airDragMutl; // 在空中增加重力
    public float maxFallSpeed; // 下落最大的速度
    public float wallSlideSpeed; // 贴墙下落速度
    public float wallJumpForce; // 贴墙跳的力
    public float dashSpeed; // 冲刺的力
    public float dashTimerSet; // 冲刺时间
    public float dashCoolTimerSet; // 冲刺的冷却时间
    public float damagedCoolDownTimer; // 受伤冷却时间(也是角色受伤后无敌的时间)

    public Vector2 wallJumpDirection;
    public Color defaultColor;
    public Color damagedColor;
    public Image blackMask;

    public Transform groundCheck;
    public Transform wallCheck;

    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    private Animator animator;
    private Vector2 moveInputDirection;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSprite;

    public Rigidbody2D PlayerRb
    {
        get
        {
            return playerRb;
        }
    }


    private int jumpCount;
    private float jumpTimer;
    private float jumpLongTimer;
    private float turnTimer;
    private int facingDirecion;

    private float dashTimer;
    private float dashCoolTimer;

    private bool canActive; // 用于控制角色能不能活动
    public bool CanActive
    {
        get
        {
            return canActive;
        }
    }

    private bool isWalking;
    private bool isTurn;
    private bool isLand;
    private bool canMove;
    private bool canFlip;
    private bool canNormalJump;
    private bool canLongJump;
    private bool canWallJump;
    private bool canDash;
    private bool isDoubleJumping;
    public bool IsDoubleJumping
    {
        get
        {
            return isDoubleJumping;
        }

        set
        {
            isDoubleJumping = value;
        }
    }
    private bool isAttemptingToJump;// 尝试跳跃
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJump; // 判断蹬墙跳
    private bool isDashing; // 判断冲刺
    private bool isCanPlayLandMusic;// 运用一个bool值来限定每次落地只播放一次声音

    private bool isDamaged;
    public bool IsDamaged
    {
        get
        {
            return isDamaged;
        }
    }

    private bool canDamaged; // 玩家受伤后有一秒的无敌时间,运用此参数限制敌人攻击
    public bool CanDamaged
    {
        get
        {
            return canDamaged;
        }
    }


    private bool isDead;
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    private bool isGround;
    public bool IsGround
    {
        get
        {
            return isGround;
        }
    }

    private bool isFackingLeft;
    public bool IsFackingLeft
    {
        get { return isFackingLeft; }
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        playerRb.gravityScale = defaultGravityScale;

        wallJumpDirection.Normalize();
        InitPlayer();
    }

    private void Update()
    {
        if (canActive && !isDead)
        {
            CheckInput();
            CheckIsCanJump();
            CheckIsWallSliding();
            CheckMovementDirection();
            CheckJump();
            CheckDash();
            IsLand();
            CheckPauseMenu();
        }

        if (!isDead)
        {
            CheckIsSurrounding();
            UpdateAnimators();
        }

        if (isDamaged)
        {
            WhenDamagedResetPlayer();
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        FallingAddGravity();
    }

    // 初始化玩家的各种值
    private void InitPlayer()
    {
        if(facingDirecion == -1)
        {
            Flip();
        }
        else
        {
            facingDirecion = 1;
        }
        canNormalJump = true;
        canLongJump = true;
        canMove = true;
        canFlip = true;
        canActive = true;
        canDamaged = true;
        isDead = false;
    }


    public void EnableActive()
    {
        canActive = true;
    }

    public void DisableActive()
    {
        canActive = false;
        isTurn = false;
    }

    public void WhenDamagedResetPlayer()
    {
        isWalking = false;
        isDoubleJumping = false;
        isTouchingWall = false;
        isWallSliding = false;
        dashTimer = -1;
    }

    // 获取用户输入
    private void CheckInput()
    {
        moveInputDirection.x = Input.GetAxisRaw("Horizontal");

        if (moveInputDirection.x != 0 && isTouchingWall)
        {
            // 在蹬墙跳的空中不能移动
            if (!isGround && moveInputDirection.x != facingDirecion)
            {
                canMove = false;
                canFlip = false;
                turnTimer = turnTimerSet;
            }
        }

        if (!canMove && !isDashing)
        {
            turnTimer -= Time.deltaTime;

            // 通过TurnTimer来决定玩家什么时候才能移动
            if (turnTimer < 0)
            {
                canFlip = true;
                canMove = true;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGround && (jumpCount > 0 || isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (Input.GetButton("Jump") && jumpLongTimer < jumpLongTimeThreshold && jumpCount == 1)
        {
            LongJump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canDash && !isDashing && !isTouchingWall)
                AttempToDash();
        }
    }

    // 获取暂停按键
    private void CheckPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.gameObject.SetActive(true);
            TimeStop.instance.StopTime();
        }
    }

    // 判断朝向
    private void CheckMovementDirection()
    {
        if (!isWallSliding && canFlip)
        {
            if (isFackingLeft && moveInputDirection.x > 0)
            {
                Flip();
            }
            else if (!isFackingLeft && moveInputDirection.x < 0)
            {
                Flip();
            }
            else
                isTurn = false;
        }
    }

    // 检查给定范围的layer
    private void CheckIsSurrounding()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsWall);
    }

    // 检查是否是停留在墙上
    private void CheckIsWallSliding()
    {
        // 确保玩家下落时可以停留在墙上
        if (isTouchingWall && playerRb.velocity.y < 0)
        {
            isWallSliding = true;
            isWallJump = false;
        }
        else
        {
            AudioManager.instance.Stop("WallSlideMusic");
            isWallSliding = false;
        }

        // 在蹬墙跳的阶段里保证不能长跳
        if (isGround && !isTouchingWall)
        {
            canLongJump = true;
        }

    }

    // 检查是否能跳跃
    private void CheckIsCanJump()
    {
        // 确保玩家在下落时并且落地才能跳跃,或者在贴墙下落时,初始化跳跃次数
        if ((isGround && playerRb.velocity.y <= 0.1f) || isWallSliding)
        {
            isDoubleJumping = false;
            jumpCount = amoutJumpCount;
            jumpLongTimer = 0;
        }

        if (isTouchingWall)
        {
            isDoubleJumping = false;
            canWallJump = true;
            canLongJump = false;
        }

        if (jumpCount <= 0)
        {
            canNormalJump = false;
        }
        else
            canNormalJump = true;
    }

    // 检查是否结束冲刺
    private void CheckDash()
    {
        // 不能连续冲刺
        if (dashCoolTimer > 0)
        {
            dashCoolTimer -= Time.deltaTime;
        }
        else
        {
            canDash = true;
        }


        if (dashTimer > 0)
        {
            canMove = false;
            canFlip = false;
            PlayerRb.velocity = new Vector2(dashSpeed * facingDirecion, 0);
            dashTimer -= Time.deltaTime;
        }

        if (dashTimer < 0)
        {
            isDashing = false;
        }

        if (isTouchingWall)
        {
            canMove = true;
            canFlip = true;
        }
    }

    // 冲刺
    private void AttempToDash()
    {
        AudioManager.instance.Play("DashMusic");
        canDash = false;
        isDashing = true;
        dashTimer = dashTimerSet;
        dashCoolTimer = dashCoolTimerSet;
    }

    // 移动
    private void ApplyMovement()
    {
        // 空中移动,松开移动键时不会立马减速,增加重力
        if (canMove && !isGround && !isWallSliding && moveInputDirection.x == 0 && !isDead && canActive)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x * airDragMutl, playerRb.velocity.y);
        }
        // 地面移动
        else if (canMove && !isDead && canActive)
        {
            playerRb.velocity = new Vector2(moveInputDirection.x * walkSpeed, playerRb.velocity.y);
        }

        // 判断播放走路动画
        if (PlayerRb.velocity.x != 0 && isGround && !isDead && canActive)
        {
            AudioManager.instance.Play("RunMusic");
            isWalking = true;
        }
        else
        {
            AudioManager.instance.Stop("RunMusic");
            isWalking = false;
        }


        // 贴墙向下移动
        if (isWallSliding)
        {
            // 当玩家
            if (playerRb.velocity.y < -wallSlideSpeed)
            {
                AudioManager.instance.Play("WallSlideMusic");
                playerRb.velocity = new Vector2(playerRb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    // 检查切换跳跃方式
    private void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (!isGround && isTouchingWall && moveInputDirection.x != 0 && moveInputDirection.x != facingDirecion)
            {
                WallJump();
            }
            else
            {
                NormalJump();
            }
        }

        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    // 地面跳跃
    private void NormalJump()
    {
        if (canNormalJump && !isWallSliding && !isDashing)
        {
            // 一段跳
            if (jumpCount == 2)
            {
                isCanPlayLandMusic = true;
                AudioManager.instance.Play("JumpMusic");
                playerRb.velocity = new Vector2(playerRb.velocity.x, jumpSpeed);
                jumpCount--;
            }
            // 二段跳
            else if (jumpCount == 1)
            {
                isDoubleJumping = true;
                playerRb.velocity = new Vector2(playerRb.velocity.x, doubleJumpSpeed);
                AudioManager.instance.Play("DoubleJumpMusic");
                animator.SetTrigger("DoubleJump");
                wingEffectAnim.SetTrigger("DoubleJump");
                GameObject doubleJumpEffect = Instantiate(doubleJumpParticle, transform.position, Quaternion.identity);
                Destroy(doubleJumpEffect, 1.5f);
                jumpCount--;
            }

            jumpTimer = 0;
            isAttemptingToJump = false;
        }
    }

    // 贴墙跳
    private void WallJump()
    {
        if (canWallJump)
        {
            // 高处下落会有加速度下坠,y赋值0保证每次跳跃力度相同
            playerRb.velocity = new Vector2(playerRb.velocity.x, 0.0f);
            isWallSliding = false;
            jumpCount = 0;

            AudioManager.instance.Play("WallJumpMusic");
            playerRb.velocity = new Vector2(wallJumpDirection.x * wallJumpForce * moveInputDirection.x, wallJumpDirection.y * wallJumpForce);

            jumpTimer = 0;
            isAttemptingToJump = false;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            isWallJump = true;
        }
    }

    // 长跳
    private void LongJump()
    {
        if (canNormalJump && canLongJump)
        {
            jumpLongTimer += Time.deltaTime;
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpSpeed);
        }
    }

    private void UpdateAnimators()
    {
        animator.SetBool("IsDashing", isDashing);
        animator.SetBool("IsWallJump", isWallJump);
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsGround", isGround);
        animator.SetBool("IsTurn", isTurn);
        animator.SetBool("IsWallSliding", isWallSliding);
        animator.SetFloat("yVelocity", playerRb.velocity.y);
        animator.SetBool("IsDoubleJumping", isDoubleJumping);
        animator.SetBool("IsDamaged", isDamaged);
    }

    //翻转角色
    public void Flip()
    {
        isTurn = true;
        facingDirecion *= -1;
        isFackingLeft = !isFackingLeft;
        transform.Rotate(0, 180, 0); // 表示绕y轴旋转180°(正轴旋转到负轴,反之亦然)
    }

    // 下落时增加重力
    private void FallingAddGravity()
    {
        // 如果玩家处于下落状态,增加重力
        if (playerRb.velocity.y < -1)
        {
            AudioManager.instance.Play("FallingMusic");
            SetGravityScale(defaultGravityScale * jumpFallingMult);
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Max(playerRb.velocity.y, -maxFallSpeed));
        }
        else
        {
            AudioManager.instance.Stop("FallingMusic");
            SetGravityScale(defaultGravityScale);
        }
    }

    // 判断下落时的高度是否需要落地动画
    private void IsLand()
    {
        if (Mathf.Abs(playerRb.velocity.y) >= LandSpeed)
            isLand = true;
        else
            isLand = false;

        animator.SetBool("IsLand", isLand);

        if (isCanPlayLandMusic && isGround && jumpCount == 2)
        {
            isCanPlayLandMusic = false;
            AudioManager.instance.Play("LandMusic");
        }
    }

    // 更改玩家重力
    private void SetGravityScale(float gravityScale)
    {
        playerRb.gravityScale = gravityScale;
    }

    // 当玩家受伤时
    public void Damaged()
    {
        isDamaged = true;
        animator.SetTrigger("Damaged");
        AudioManager.instance.Play("PlayerDamagedMusic");
        GameObject _damagedEffect = Instantiate(damagedEffect, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        Destroy(_damagedEffect, 1.0f);
        StartCoroutine(OnDamaged());
    }

    public void Dead()
    {
        isDead = true;
        animator.SetBool("IsDead", isDead);
        StartCoroutine(OnDead());
    }

    public void OnSpike()
    {
        if (!isDead)
            StartCoroutine(OnEnterSprike());
        else
            Dead();
    }

    private IEnumerator OnDamaged()
    {
        canDamaged = false;
        TimeStop.instance.StopTime(0.05f, 5);

        // 运用DoTween插件控制玩家的图片交换显示
        var sequence = DOTween.Sequence();
        sequence.Append(playerSprite.material.DOColor(damagedColor, 0.15f));
        sequence.Append(playerSprite.material.DOColor(defaultColor, 0.15f));
        sequence.SetLoops(10); // 表示队列播放10次

        yield return new WaitForSeconds(damagedCoolDownTimer);

        canDamaged = true;
        sequence.Complete(); // 在规定时间后停止队列(立即完成动画)
    }

    private IEnumerator OnDead()
    {
        playerRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.0f);

        var sequence = DOTween.Sequence();
        sequence.Append(blackMask.DOFade(1f, 1f));
        sequence.AppendCallback(() =>
        {
            sequence.Append(blackMask.DOFade(0f, 1.5f));
        });
        CharacterResetPosition.Instance.RespawnPlayer();
        playerRb.velocity = Vector2.zero;
        InitPlayer();
        PlayerData.Instance.Health = 5;
        animator.SetBool("IsDead", isDead);

    }

    // 踩到地刺时的协程
    private IEnumerator OnEnterSprike()
    {
        playerRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);


        var sequence = DOTween.Sequence();
        sequence.Append(blackMask.DOFade(1f, 1f));
        sequence.AppendCallback(() =>
        {
            sequence.Append(blackMask.DOFade(0f, 1.5f));
        });
        CharacterResetPosition.Instance.RespawnPlayer();
        playerRb.velocity = Vector2.zero;
        if(facingDirecion == -1)
        {
            Flip();
        }
    }

    /*
     * * ************************动画事件**************************
     */

    public void EnableFilpAnimEvent()
    {
        canFlip = true;
    }

    public void DisableFilpAnimEvent()
    {
        canFlip = false;
    }

    // 受伤动画结束可以移动
    public void EnableActiveAnimEvent()
    {
        EnableActive();
        isDamaged = false;
    }

    // 受伤时不能移动
    public void DisableActiveAnimEvent()
    {
        DisableActive();
    }

    public void DoubleJumpFinishAnimEvent()
    {
        isDoubleJumping = false;
    }


    public void WallJumpAnimEvent()
    {
        isWallJump = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + transform.right.x * wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }

}
