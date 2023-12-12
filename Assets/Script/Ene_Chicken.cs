using UnityEngine;

public class Ene_Chicken : Enemy
{
    [Header("Move Info")]
    [SerializeField] private float speed;
    [SerializeField] private float idleTime = 2;
    [SerializeField] private float idleTimeCounter;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {

        

        idleTimeCounter -= Time.deltaTime;

        CollisionCheck();

        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }

        if (idleTimeCounter <= 0)
            rb.velocity = new Vector2(speed * facingDir, rb.velocity.y);
    }
}
