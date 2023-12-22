using UnityEngine;

public class Ene_Trunk : Enemy
{
    [Header("Trunk Specifics")]
    [SerializeField] private float checkRadius;

    private bool playerDetected;

    [Header("Bullet Trunk")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackCooldownCounter;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform buttletOrigin;
    [SerializeField] private float bulletSpeed;

    protected override void Start()
    {
        base.Start();
    }




    void Update()
    {
        if (isDead) return;
        CollisionCheck();

        if (!canMove)
        {
            rb.velocity = new Vector2(0, 0);
        }


        attackCooldownCounter -= Time.deltaTime;

        if (playerDetection)
        {
            if (attackCooldownCounter < 0)
            {
                attackCooldownCounter = attackCooldown;
                anim.SetTrigger("Attack");
                canMove = false;
            }

        }
        else
        {
            WalkAround();
        }
    }

    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, buttletOrigin.transform.position, buttletOrigin.transform.rotation);

        newBullet.GetComponent<Ene_Bullet>().SetupSpeed(bulletSpeed * facingDir, 0f);
        Destroy(newBullet, 3f);
    }


    protected override void CollisionCheck()
    {
        base.CollisionCheck();
        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
