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
    private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Wall Slide")]
    private bool isWallDetected;
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed;



    private Animator anim;

    private bool isFacingRight = true;
    private int facingDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpCount = jumpMax;

    }


    void Update()
    {
        CollisionChecks();          // Check Ground,
        InputCheck();               // Input from player
        Move();                     // Move (rb.velocity * moveInput)
        DoubleJump();               // Jumpcount > 0 and isGround
        AnimController();           // Animation setup
        FlipController();           // Facing = Rotate transform
        WallSliding();              // Walldetected = true and velocity.y < 0 then WallSlide



    }

    private void WallSliding()
    {
        if (isWallDetected && rb.velocity.y < 0)
        {

            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * wallSlidingSpeed);
            
                

        }
        if (!isWallDetected)
        {

            isWallSliding = false;

            
        }
    }

    private void AnimController()
    {

        anim.SetFloat("xInput", rb.velocity.x);

        anim.SetFloat("yInput", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isWallSliding);

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
        /*if (isWallDetected)
        {
            jumpCount++;
        }*/


    }

    private void InputCheck()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetButtonDown("Jump");

        if (Input.GetAxis("Vertical") < 0)
        {

            isWallDetected = false;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }


    private void FlipController()
    {
        if (isFacingRight && xInput < 0) // Faceing Right = false and Move to Left
        {
            Flip();
        }
        else if (!isFacingRight && xInput > 0) // Faceing Right = true and Move to Right
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1; //Facing Dir to Left
        isFacingRight = !isFacingRight;         // Facing Left
        transform.Rotate(0, 180, 0);            // Rotate to Left
    }

    private void CollisionChecks()
    {
        //Ground check line (position, Vector.down, Vector lenght, Layer)
        isGrounded = Physics2D.Raycast(transform.position,      // form position of line middle character
                                        Vector2.down,           // to Vector draw to down
                                        groundCheckDistance,    // Vector length by float
                                        whatIsGround);          // Layer of Ground set in Unity layer
        //Wall check line (position, Vector.right, Vector lenght, Layer)
        isWallDetected = Physics2D.Raycast(transform.position,
                                        Vector2.right,
                                        wallCheckDistance,
                                        whatIsGround);

    }

    private void OnDrawGizmos()
    {
        // DrawLine (position A from, to position B)
        Gizmos.DrawLine(transform.position,                     // position A
                        new Vector2(transform.position.x,       // position B (x, y to vector Length can input)
                                    transform.position.y -
                                    groundCheckDistance));

        // DrawLine (position A from, to position B)
        Gizmos.DrawLine(new Vector2(transform.position.x + wallCheckDistance, transform.position.y), transform.position);


    }
}
