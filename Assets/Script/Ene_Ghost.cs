using UnityEngine;

public class Ene_Ghost : Enemy
{
    [Header("Ghost specsifics")]
    [SerializeField] private float activeTime;
    [SerializeField] private float activeTimeCounter = 4;

    private Transform player;
    private SpriteRenderer sr;
    [SerializeField] private float[] xOffset;

    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        aggresive = true;
        invincible = true;  
        player = GameObject.Find("Player").transform;
    }



    private void Update()
    {
        activeTimeCounter -= Time.deltaTime;
        idleTimeCounter -= Time.deltaTime;

        if (activeTimeCounter > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && aggresive)
        {
            anim.SetTrigger("Desappear");
            aggresive = false;
            idleTimeCounter = idleTime;
        }
        if (activeTimeCounter < 0 && idleTimeCounter < 0 && !aggresive)
        {
            ChoosePosition();
            anim.SetTrigger("Appear");
            aggresive = true;
            activeTimeCounter = activeTime;
        }

        if(facingDir == -1 && transform.position.x < player.transform.position.x)
        {
            Flip();
        }
        else if(facingDir == 1 && transform.position.x > player.transform.position.x)
        {
            Flip();
        }
    }

    private void ChoosePosition()
    {
        float _xOffset = xOffset[Random.Range(0, xOffset.Length)];
        float _yOffset = Random.Range(-10, 10);

        transform.position = new Vector2(player.transform.position.x + _xOffset , player.transform.position.y + _yOffset);
    }

    private void Desappear()
    {
        sr.enabled = false;
    }
    private void Appear()
    {
        sr.enabled = true;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (aggresive)
        {
            base.OnTriggerStay2D(collision);

        }
    }
}
