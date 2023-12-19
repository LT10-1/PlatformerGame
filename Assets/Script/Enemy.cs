using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;

    protected int facingDir = -1;

    [Header("Move Info")]
    [SerializeField] public float speed;
    [SerializeField] protected float idleTime = 1;
    [SerializeField] protected float idleTimeCounter;


    [SerializeField] protected LayerMask whatisGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;

    protected bool wallDetected;
    protected bool groundDetected;

    [HideInInspector] public bool invincible = false;
    [HideInInspector] protected bool canMove = true;
    [SerializeField] protected bool aggresive;


    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void WalkAround()
    {
        idleTimeCounter -= Time.deltaTime;

        if (idleTimeCounter <= 0 && canMove)
            rb.velocity = new Vector2(speed * facingDir, 0f);
        else
            rb.velocity = Vector2.zero;

        if (wallDetected || !groundDetected)
        {
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
            transform.localScale = new Vector2(1, 1);
            rb.velocity = new Vector2(0, 0);
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
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(groundCheck != null)
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        if(wallCheck != null)
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
}
