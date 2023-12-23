using UnityEngine;

public class Ene_Bat : Enemy
{

    [SerializeField] private Transform[] idlePoint;
    [SerializeField] private float checkRadius;
    private bool playerDetected;
    private Transform player;

    private Vector2 destination;

    private bool canBeAngryMode = true;
    [SerializeField] private float defaultSpeed;

    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private float waitTimeAfterDetection = 3f; // Thời gian đợi sau khi phát hiện người chơi
    [SerializeField] private float detectionTimeCounter; // Bộ đếm thời gian sau khi phát hiện

    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        defaultSpeed = speed;
        destination = idlePoint[0].position;
        transform.position = idlePoint[0].position;
        detectionTimeCounter = waitTimeAfterDetection;
    }

    // Update is called once per frame
    void Update()
    {
        idleTimeCounter -= Time.deltaTime;
        anim.SetBool("canBeAngryMode", canBeAngryMode);
        anim.SetFloat("speed", speed);

        if (idleTimeCounter > 0)
            return;

        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);

        if (playerDetected && !angryMode && canBeAngryMode && detectionTimeCounter > 0)
        {
            detectionTimeCounter -= Time.deltaTime;
            
           
            

            if (detectionTimeCounter > 0)
            {
               
                destination = idlePoint[0].position;
                
                rb.velocity = new Vector2(0, 0);
                enemyRenderer.color = Color.red;
            }

            else if (detectionTimeCounter <= 0) 
            {

                destination = player.transform.position;
                
                angryMode = true; 
               
                canBeAngryMode = false;

                detectionTimeCounter = waitTimeAfterDetection;

                enemyRenderer.color = Color.red;

            }
            

        }

        else if (angryMode == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                angryMode = false;

                
                destination = idlePoint[0].position;
                speed = speed * .5f;
                enemyRenderer.color = Color.white;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                if (!canBeAngryMode)
                    idleTimeCounter = idleTime;
                canBeAngryMode = true;
                speed = defaultSpeed;
                enemyRenderer.color = Color.white;
            }
        }

        FlipController();
    }

    private void FlipController()
    {

        if (facingDir == -1 && transform.position.x < destination.x)
        {
            Flip();
        }
        else if (facingDir == 1 && transform.position.x > destination.x)
        {
            Flip();
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
