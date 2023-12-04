using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Move Basic Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float xInput;
    private bool yInput;
    private int jumpMax = 2;
    private int jumpCount;
    private bool isFacingRight = true;
    private bool canMove;
    private int facingDir = 1;

    [Header("Check Collision")]
    public Transform wallCheck;
    public Transform groundCheck;
    [SerializeField] private Vector2 wallCheckSize;
    [SerializeField] private Vector2 groundCheckSize;
    private bool isWallDetected;
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;


    [Header("Wall Slide")]
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed;

    [Header("Wall Jump")]
    private bool isWallJump;
    [SerializeField] private Vector2 wallJumpDirection;

    [Header("PlayerHit Info")]
    private bool isHit;
    private bool canHit = true;
    [SerializeField] private Vector2 HitDirection;
    [SerializeField] private float HitTime;
     private float HitTimeCounter;
    [SerializeField] private float CooldownTimePlayerHit;

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

        CollisionChecks();
        AnimController();

        if (Input.GetKeyDown(KeyCode.K))
        {
            
            PlayerHit();

        }
        if(isHit)
            return;

        InputCheck();
        Move();
        DoubleJump();
        FlipController();

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
        }

    }

    private IEnumerator EnableMovementAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // Đợi 0.1 giây trước khi cho phép di chuyển
        canMove = true;
        isWallJump = false; // Đặt isWallJump thành false sau khi rời khỏi tường
    }

    public void PlayerHit()
    {
        if(canHit && HitTimeCounter < 0)
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
            jumpCount = jumpMax;


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
        }
        else if (isGrounded && yInput)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }
        else if (!isGrounded && yInput && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--; // Giảm jumpCount mỗi khi nhảy trong không trung
        }
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
    }
    private void CollisionChecks()
    {

        //Ground check box
        isGrounded = Physics2D.BoxCast(groundCheck.position, groundCheckSize, 0, Vector2.zero, whatIsGround);


        //Wall check box
        isWallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, whatIsGround);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
