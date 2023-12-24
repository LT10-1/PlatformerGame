using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Enemy : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;
    protected CapsuleCollider2D CapCollider;


    protected int facingDir = -1;

    [Header("Move Info")]
    [SerializeField] public float speed;
    [SerializeField] protected float idleTime = 1;
    [SerializeField] protected float idleTimeCounter;
    [SerializeField] protected float FlipTime;
    [SerializeField] protected float FlipTimeCounter;

    [SerializeField] protected LayerMask whatisGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;

    protected bool wallDetected;
    protected bool groundDetected;
    protected RaycastHit2D playerDetection;
    [SerializeField] protected LayerMask whatIsPlayer;

    [HideInInspector] public bool invincible = false;
    [HideInInspector] protected bool canMove = true;
    [SerializeField] protected bool angryMode;

    public bool isDead;

    protected Transform player;
    

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = PlayerManager.instance.currentPlayer.transform;
        CapCollider = GetComponent<CapsuleCollider2D>();
        
    }

    protected virtual void WalkAround()
    {
        if (isDead) return;
        if (!canMove)
        {
            rb.velocity = new Vector2(0, 0);
        }
        canMove = true;
        anim.SetFloat("xVelocity", rb.velocity.x);
        idleTimeCounter -= Time.deltaTime;
        FlipTimeCounter -= Time.deltaTime;
        if (idleTimeCounter <= 0 && canMove)
            rb.velocity = new Vector2(speed * facingDir, 0f);
        else
            rb.velocity = Vector2.zero;

        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }
        else if(FlipTimeCounter < 0 )
        {
            FlipTime = Random.Range(3f, 10f);
            FlipTimeCounter = FlipTime;
            idleTimeCounter = idleTime;
            Flip();
        }

    }

    public virtual void Damage()
    {
        if (!invincible)
        {
            canMove = false;
            anim.SetTrigger("isHit");
            if (!canMove)
            {
                isDead = true;
                if (isDead)
                {
                    
                    transform.Rotate(new Vector3(0, 0, Random.Range(-45f,-180f)));
                    
                    rb.velocity = new Vector2(transform.position.x * -facingDir, 8f);
                    rb.gravityScale = 5f;
                    CapCollider.enabled = false;

                }
            }

        }


    }

    

    public void DestroyMe()
    {
        canMove = false;
        Destroy(gameObject);
        rb.velocity = new Vector2(0, 0);

    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {

        Player playerCollider = collision.GetComponent<Player>();
        if (playerCollider != null)
        {
            if (!playerCollider.isRoll)
                playerCollider.PlayerHit();


        }
    }




    protected virtual void Flip()
    {
        facingDir = facingDir * -1;

        transform.Rotate(0, 180, 0);            // Rotate to Left
    }

    protected virtual void CollisionCheck()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatisGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatisGround);
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 25f, whatIsPlayer);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        if (wallCheck != null)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDir, wallCheck.position.y));
        }
    }
}
