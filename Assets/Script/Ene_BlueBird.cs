using UnityEngine;

public class Ene_BlueBird : Enemy
{

    [Header("BlueBird Specific")]

    private bool cellingDetected;
    [SerializeField] private float groundBelowCheckDistance;
    [SerializeField] private float cellingCheckDistance;
    [SerializeField] private Transform positionCellingCheck;
    [SerializeField] private float flyForce;
    [SerializeField] private float flyUpForce;
    [SerializeField] private float flyDownForce;
    [SerializeField] private bool canFly = true;




    protected override void Start()
    {
        base.Start();
        flyForce = flyUpForce;
    }



    void Update()
    {
        if (isDead) return;
        CollisionCheck();

        if (cellingDetected)
        {
            flyForce = flyDownForce;
        }
        else if (groundDetected)
        {
            flyForce = flyUpForce;
        }
        if (wallDetected)
        {
            Flip();
        }
    }

    public void FlyUpEvent()
    {
        if (canFly)
            rb.velocity = new Vector2(speed * facingDir, flyForce);
    }

    protected override void CollisionCheck()
    {
        base.CollisionCheck();


        cellingDetected = Physics2D.Raycast(positionCellingCheck.position, Vector2.up, cellingCheckDistance, whatisGround);
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
        canFly = false;
        base.Damage();
        rb.gravityScale = 0;
    }
}


