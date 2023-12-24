using UnityEngine;

public class Ene_Radish : Enemy
{

    private RaycastHit2D groundBelowDetected;
    private bool cellingDetected;

    [Header("Radish Specifics")]
    [SerializeField] private float groundBelowCheckDistance;
    [SerializeField] private float cellingCheckDistance;
    [SerializeField] private float aggroTime;
    [SerializeField] private float angryTimeCounter;
    [SerializeField] private float flyUpForce = 5f;
    [SerializeField] private float flyDownForce = -5f;
    [SerializeField] private float gravityScale = -12f;
    [SerializeField] private bool onGround;
    [SerializeField] private Transform positionCellingCheck;

    protected override void Start()
    {
        base.Start();
    }



    void Update()
    {
        if (isDead) return;
        CollisionCheck();
        angryTimeCounter -= Time.deltaTime;

        if (angryTimeCounter < 0)
        {
            onGround = false;

        }

        if (!onGround)
        {
            if (groundBelowDetected && !cellingDetected)
            {

                rb.velocity = new Vector2(0, flyUpForce);
            }
            else if (cellingDetected && !groundBelowDetected)
                rb.velocity = new Vector2(0, flyDownForce);
            else if (groundBelowDetected && cellingDetected)
            {
                rb.velocity = new Vector2(-5f * -facingDir, 0f);
                if(wallDetected) 
                Flip();
            }
            else
            {
                rb.AddForce(-transform.up * 3f);

            }
        }
        else if (angryTimeCounter > 0)
        {
            rb.velocity = new Vector2(0, gravityScale);
            if (groundDetected)
            {
                WalkAround();
                onGround = true;
                rb.gravityScale = 0f;
               
            }

        }


        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("onGround", onGround);


    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();

        groundBelowDetected = Physics2D.Raycast(transform.position, Vector2.down, groundBelowCheckDistance, whatisGround);
        cellingDetected = Physics2D.Raycast(positionCellingCheck.position, Vector2.up, cellingCheckDistance , whatisGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundBelowCheckDistance));
        Gizmos.DrawLine(positionCellingCheck.position, new Vector2(positionCellingCheck.position.x, positionCellingCheck.position.y + cellingCheckDistance));

    }

    public override void Damage()
    {
        if (!onGround)
        {
            angryTimeCounter = aggroTime;
            onGround = true;

        }
        else
            base.Damage();



    }
}
