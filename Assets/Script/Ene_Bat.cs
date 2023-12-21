using UnityEngine;

public class Ene_Bat : Enemy
{
    [SerializeField] private Transform[] idlePoint;

    private Vector2 destination;
    private bool canBeAngryMode = true;
    private bool playerDetected;
    private Transform player;


    [SerializeField] private float checkRadius;
    [SerializeField] private float defaultSpeed;

    

    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        defaultSpeed = speed;
        destination = idlePoint[0].position;
        transform.position = idlePoint[0].position;
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

        if (playerDetected && !angryMode && canBeAngryMode)
        {
            angryMode = true;
            canBeAngryMode = false;
            destination = player.transform.position;

        }

        if (angryMode)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                angryMode = false;

                int i = Random.Range(0, idlePoint.Length);
                destination = idlePoint[i].position;
                speed = speed * .5f;
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
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        base.OnDrawGizmos();
    }
}
