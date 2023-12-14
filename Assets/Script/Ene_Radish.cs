using UnityEngine;

public class Ene_Radish : Enemy
{

    private RaycastHit2D groundBelowDetected;
    private bool cellingDetected;

    [Header("Radish Specifics")]
    [SerializeField] private float groundBelowCheckDistance;
    [SerializeField] private float cellingCheckDistance;
    [SerializeField] private float aggroTime;
                     private float aggroTimeCounter;
    [SerializeField] private float flyForce;
    [SerializeField] private bool aggresive;

    protected override void Start()
    {
        base.Start();
    }



    void Update()
    {
        aggroTimeCounter -= Time.deltaTime;

        if(aggroTimeCounter < 0 && !cellingDetected)
        {
            aggresive = false;
            rb.gravityScale = 1.0f;
        }

        if (!aggresive)
        {
            if (groundBelowDetected && !cellingDetected)
            {
                rb.gravityScale = 12;
                rb.velocity = new Vector2(0, flyForce);
            }
        }
        else
        {
            if (groundBelowDetected.distance <= 1.25f )
                WalkAround();

        }
        CollisionCheck();


        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("aggresive", aggresive);


    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        groundBelowDetected = Physics2D.Raycast(transform.position, Vector2.down, groundBelowCheckDistance, whatisGround);
        cellingDetected = Physics2D.Raycast(transform.position, Vector2.up, cellingCheckDistance, whatisGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundBelowCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + cellingCheckDistance));

    }

    public override void Damage()
    {
        if (!aggresive)
        {
            aggroTimeCounter = aggroTime;
            aggresive = true;
            rb.gravityScale = 12f;
        }
        else
            base.Damage();

        
        
    }
}
