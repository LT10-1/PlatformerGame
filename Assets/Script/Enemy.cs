using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;

    protected int facingDir = -1;

    [SerializeField] protected LayerMask whatisGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;

    protected bool wallDetected;
    protected bool groundDetected;

    public bool invincible;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }


    public void Damage()
    {
        if (!invincible)
        {
            anim.SetTrigger("isHit");
            transform.localScale = new Vector2(1, 1);

        }
        
   
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
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
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }
}
