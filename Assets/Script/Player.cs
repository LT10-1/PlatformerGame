using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

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

    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Transform enemyCheckRollAttack;

    [SerializeField] private float enemyCheckRadiusRollAttack;
    [SerializeField] private float wallCheckDistance;
    public Transform wallCheck;
    public Transform groundCheck;
    private bool isWallDetected;
    private bool isGrounded;



    [Header("PlayerHit")]
    [SerializeField] private Vector2 HitDirection;
    [SerializeField] private float HitTime;
    [SerializeField] private float CooldownTimePlayerHit;
    private bool isHit;
    private bool canHit = true;
    private float HitTimeCounter;

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


        CollisionChecks();
        AnimController();
        PlayerRolling();
        if (isHit)
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

            }

            if (isRoll && hasDamagedEnemyDuringRoll)
            {
                rb.velocity = new Vector2(HitDirection.x * -facingDir, RollingDir.y * 3f);
                newEnemy.Damage();

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
            isHit = true;
            HitTimeCounter = CooldownTimePlayerHit;
            rb.velocity = new Vector2(HitDirection.x * -facingDir, HitDirection.y);
            Invoke("CancelPlayerHit", HitTime);

        }


    }

    void CancelPlayerHit() => isHit = false;


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
            jumpCount -= 1;
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
        if (!isGrounded)
        {
            bufferJumpCounter = bufferJumpTime;
        }

        if (isWallSliding)
        {
            WallJumping();
        }
        else if (isGrounded && yInput)
        {
            JumpButton();
        }
        else if (!isGrounded && yInput && jumpCount > 0)
        {
            JumpButton();
        }

    }

    private void JumpButton()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

        anim.SetFloat("xInput", rb.velocity.x);

        anim.SetFloat("yInput", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isWallSliding);
        anim.SetBool("isHit", isHit);
        anim.SetBool("isRoll", isRoll);
    }
    private void CollisionChecks()
    {

        //Ground check box
        isGrounded = Physics2D.BoxCast(groundCheck.position, groundCheckSize, 0, Vector2.zero, whatIsGround);


        //Wall check box
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

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
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));


        Gizmos.DrawWireSphere(enemyCheckRollAttack.position, enemyCheckRadiusRollAttack);
    }
}