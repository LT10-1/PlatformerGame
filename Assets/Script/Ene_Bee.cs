using UnityEngine;

public class Ene_Bee : Enemy
{
    [Header("Bee Specifics")]
    [SerializeField] private Transform[] idlePoint;
    [SerializeField] private float checkRadius;
     private Transform checkPoint;
    [SerializeField] private float yOffset;
    [SerializeField] private bool playerDetected;
    [SerializeField] private Transform playerCheck;
   
    private float defaultSpeed;
    private int idlePointIndex;


    [Header("Bullet Bee")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform buttletOrigin;
    [SerializeField] private float bulletSpeed;

    protected override void Start()
    {
        base.Start();
        
        defaultSpeed = speed;
    }



    void Update()
    {
        bool idle = idleTimeCounter > 0;

        anim.SetBool("idle", idle);
        idleTimeCounter -= Time.deltaTime;
        
        if (idle)
            return;

        if (player == null)
            return;

        playerDetected = Physics2D.OverlapCircle(playerCheck.position, checkRadius, whatIsPlayer);

        if (playerDetected && !angryMode)
        {
            angryMode = true;
            speed = speed * 1.5f;
        }

        if (!angryMode)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, idlePoint[idlePointIndex].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, idlePoint[idlePointIndex].position) < .1f)
            {
                idlePointIndex++;
                if (idlePointIndex >= idlePoint.Length)
                    idlePointIndex = 0;
            }
        }
        else
        {
           

            Vector2 newPosition = new Vector2(idlePoint[idlePointIndex].position.x, idlePoint[idlePointIndex].position.y);
            transform.position = Vector2.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

            float xDifference = transform.position.x - idlePoint[idlePointIndex].position.x;

            if (Mathf.Abs(xDifference) < 0.15f)
            {
                anim.SetTrigger("Attack");
                
            }

        }
    }

    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, buttletOrigin.transform.position, buttletOrigin.transform.rotation);
        newBullet.GetComponent<Ene_Bullet_Bee>().SetupSpeed(0f, -bulletSpeed);

        idleTimeCounter = idleTime;
        angryMode = false;
        speed = defaultSpeed;
        Destroy(newBullet, 4f);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(playerCheck.position, checkRadius);
    }

}


