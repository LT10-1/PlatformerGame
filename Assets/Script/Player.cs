using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    public int fruit;
    private bool isDead = false;

    [Header("Move Basic")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float xInput;
    private bool yInput;
    private int jumpMax = 2;
    private int jumpCount;
    private bool isFacingRight = true;
    private bool canMove;
    private int facingDir = 1;
    [SerializeField] private float bufferJumpTime;
    [SerializeField] private float bufferJumpCounter;




    [Header("Wall Slide")]
    [SerializeField] private float wallSlidingSpeed;
    private bool isWallSliding;

    [Header("Wall Jump")]
    [SerializeField] private Vector2 wallJumpDirection;
    private bool isWallJump;

    [Header("Check Collision")]

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;

    [SerializeField] private Transform enemyCheckRollAttack;

    [SerializeField] private float enemyCheckRadiusRollAttack;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private float groundCheckDistance;
    public Transform wallCheck;
    public Transform groundCheck;
    private bool isWallDetected;
    private bool isGrounded;



    [Header("PlayerHit")]
    [SerializeField] private Vector2 HitDirection;
    [SerializeField] private float HitTime;
    [SerializeField] private float CooldownTimePlayerHit;
    public bool playerisHit;
    public bool canHit = true;
    [SerializeField] private float HitTimeCounter;

    [Header("PlayerRoll")]
    [SerializeField] private Vector2 RollingDir;
    public bool isRoll;
    private bool RollButton;
    [SerializeField] private LayerMask whatisEnemy;

    [SerializeField] private float RollTimeCounter;
    [SerializeField] private float RollTimeCooldown;
    [SerializeField] private float RollTimeAttack;
    [SerializeField] public float RollTimeAttackCounter;

    private bool hasDamagedEnemyDuringRoll = true;


    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpCount = jumpMax;

    }


    void Update()
    {

        HitTimeCounter -= Time.deltaTime;
        RollTimeCounter -= Time.deltaTime;
        bufferJumpCounter -= Time.deltaTime;
        RollTimeAttackCounter -= Time.deltaTime;


        AnimController();
        if (isDead)
        {
            return;
        }
        CollisionChecks();
        PlayerRolling();
        if (playerisHit)
            return;


        if (isRoll)
            return;
        if (!isRoll)
        {
            hasDamagedEnemyDuringRoll = true;
        }

        InputCheck();
        Move();
        DoubleJump();
        FlipController();

    }




    private void OnTriggerEnter2D(Collider2D rollCollider)
    {
        rollCollider = Physics2D.OverlapCircle(enemyCheckRollAttack.position, enemyCheckRadiusRollAttack, whatisEnemy);

        if (rollCollider != null)
        {
            Enemy newEnemy = rollCollider.GetComponent<Enemy>();
            if (newEnemy.invincible)
            {

                return;
            }

            if (rb.velocity.y < 0 && !isRoll)
            {
                JumpButton();
                newEnemy.Damage();
                HitTimeCounter = HitTime;

            }

            if (isRoll && hasDamagedEnemyDuringRoll)
            {
                rb.velocity = new Vector2(HitDirection.x * -facingDir, RollingDir.y * 3f);
                newEnemy.Damage();
                HitTimeCounter = HitTime;
                hasDamagedEnemyDuringRoll = false; // Đánh dấu đã gây sát thương
            }


        }
    }




    private void PlayerRolling()
    {
        if (RollButton && RollTimeCounter < 0)
        {
            isRoll = true;
            canMove = false;
            RollTimeCounter = RollTimeCooldown;
            RollTimeAttackCounter = RollTimeAttack;
            if (isRoll)
            {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(RollingDir.x * facingDir, 0f);
            }
        }


    }

    void CancelPlayerRoll()
    {
        isRoll = false;
        canMove = true;

        rb.gravityScale = 4f;

    }


    private IEnumerator EnableMovementAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Đợi 0.1 giây trước khi cho phép di chuyển
        canMove = true;
        isWallJump = false; // Đặt isWallJump thành false sau khi rời khỏi tường
    }



    public void PlayerHit()
    {

        if (canHit && HitTimeCounter < 0)
        {
            playerisHit = true;
            HitTimeCounter = CooldownTimePlayerHit;
            rb.velocity = new Vector2(HitDirection.x * -facingDir, HitDirection.y);
            Invoke("CancelPlayerHit", HitTime);
            fruit--;
            GetComponent<CameraShake>().ScreenShake(-facingDir);
            if (fruit < 0)
            {
                canMove = false;
                rb.velocity = new Vector2 (0,0);

                Invoke("playdeath", 1f);


                if (isDead)
                {
                    rb.velocity = new Vector2(0, 0);
                    Destroy(gameObject);

                }

            }
        }


    }

    private void playdeath() => isDead = true;

    public void CancelPlayerHit() => playerisHit = false;


    private void WallSliding()
    {
        if (isWallDetected && rb.velocity.y < 0)
        {

            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlidingSpeed));

            jumpCount = jumpMax;
        }

        else
        {
            isWallSliding = false;
        }
    }

    private void WallJumping()
    {
        if (isWallSliding)
        {

            rb.velocity = new Vector2(wallJumpDirection.x * -facingDir, wallJumpDirection.y);
            canMove = false;
            isWallJump = true;

        }


    }



    private void DoubleJump()
    {
        if (isGrounded) // is Ground then jumpcount = max
        {
            jumpCount = jumpMax;
        }

        if (yInput && jumpCount > 0) // Jump function call and jumpcount - 1, if Button Press (yInput) and jumpCount > 0
        {
            Jump();

        }


    }

    private void InputCheck()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetButtonDown("Jump");
        RollButton = Input.GetButtonDown("Fire3");
        if (Input.GetAxis("Vertical") < 0)
            isWallDetected = false;

    }

    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

        }
    }


    private void Jump()
    {
        if (isWallSliding)
        {
            WallJumping();
            jumpCount -= 1; // Decrease jumpCount here after wall jump.
        }
        else if (isGrounded && yInput)
        {
            JumpButton();

        }
        else if (!isGrounded && yInput && jumpCount > 0)
        {
            JumpButton();
            jumpCount -= 1; // And here
        }
    }



    private void JumpButton()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount -= 1; // Decrease jumpCount here after a successful jump.
    }

    public void Push(float pushForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, pushForce);
    }

    private void FlipController()
    {
        if (isFacingRight && rb.velocity.x < 0) // Faceing Right = false and Move to Left
            Flip();
        else if (!isFacingRight && rb.velocity.x > 0) // Faceing Right = true and Move to Right
            Flip();

    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        isFacingRight = !isFacingRight;         // Facing Left
        transform.Rotate(0, 180, 0);            // Rotate to Left
    }
    private void AnimController()
    {
        anim.SetBool("isDead", isDead);
        anim.SetFloat("xInput", rb.velocity.x);

        anim.SetFloat("yInput", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isWallSliding);
        anim.SetBool("isHit", playerisHit);
        anim.SetBool("isRoll", isRoll);
    }
    private void CollisionChecks()
    {

        //Ground check box
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);


        //Wall check box
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsWall);

        if (isWallDetected)
        {

            WallSliding();
            if (yInput && isWallSliding)
            {
                WallJumping();
            }
        }
        else
        {
            isWallSliding = false;
            if (isWallJump)
            {
                // Cho phép một khoảng thời gian ngắn để nhân vật di chuyển sau wall jump
                StartCoroutine(EnableMovementAfterDelay());
            }
        }

        if (isGrounded)
        {


            canMove = true;
            isWallJump = false; // Đặt isWallJump thành false khi chạm đất

            if (bufferJumpCounter > 0)
            {
                bufferJumpCounter = -1;
                Jump();
            }
        }



    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));


        Gizmos.DrawWireSphere(enemyCheckRollAttack.position, enemyCheckRadiusRollAttack);
    }
}