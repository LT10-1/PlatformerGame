using UnityEngine;


public class Ene_Rino : Enemy
{
    [Header("Move Info")]
    [SerializeField] private float speed;
    [SerializeField] private float speedAngry;
    [SerializeField] private float idleTime = 1;
    [SerializeField] private float idleTimeCounter;

    [SerializeField] private float shockTime;
    private float shockTimeCounter;


    [SerializeField] private LayerMask whatToIgnore;
    private RaycastHit2D playerDetection;
    private bool angryMode;


    protected override void Start()
    {
        base.Start();
        invincible = true;
    }


    void Update()
    {
        playerDetection = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 25f, ~whatToIgnore);

        if (playerDetection.collider.GetComponent<Player>() != null)
        {

            angryMode = true;

        }

        if (!angryMode)
        {

            if (idleTimeCounter <= 0)
                rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
            else
                rb.velocity = new Vector2(0, 0);

            idleTimeCounter -= Time.deltaTime;

            if (wallDetected || !groundDetected)
            {
                idleTimeCounter = idleTime;
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(speedAngry * facingDir, rb.velocity.y);

            if (wallDetected && invincible)
            {
                invincible = false;
                transform.localScale = new Vector2(1, -1);
                shockTimeCounter = shockTime;
                Flip();

            }


            if (shockTimeCounter <= 0 && !invincible)
            {
                transform.localScale = new Vector2(1, 1);
                invincible = true;
                Flip();
                angryMode = false;
            }
            shockTimeCounter -= Time.deltaTime;
        }



        CollisionCheck();
        AnimatorController();

    }

    private void AnimatorController()
    {
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetection.distance * facingDir, wallCheck.position.y));
    }

}
