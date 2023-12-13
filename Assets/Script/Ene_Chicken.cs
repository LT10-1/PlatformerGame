using UnityEngine;

public class Ene_Chicken : Enemy
{
    [Header("Move Info")]
    [SerializeField] private float speed;
    private float idleTime = 1;
    private float idleTimeCounter;
    

    protected override void Start()
    {
        base.Start();
        
    }

    private void Update()
    {

        idleTimeCounter -= Time.deltaTime;

        if (idleTimeCounter <= 0)
            rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
        else
            rb.velocity = Vector2.zero;

        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }


        CollisionCheck();
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
