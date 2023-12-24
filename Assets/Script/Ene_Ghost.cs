using UnityEngine;

public class Ene_Ghost : Enemy
{
    [Header("Ghost specsifics")]
    [SerializeField] private float activeTime;
    [SerializeField] private float activeTimeCounter = 4;

    
    private SpriteRenderer sr;
    [SerializeField] private float[] xOffset;

    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
        angryMode = false;
         
        
    }



    private void Update()
    {
        if (player == null)
        {
            anim.SetTrigger("Desappear");
            return;
        }
        if(isDead)
        {
            return;
        }
        activeTimeCounter -= Time.deltaTime;
        idleTimeCounter -= Time.deltaTime;

        if (activeTimeCounter > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && angryMode)
        {
            anim.SetTrigger("Desappear");
            angryMode = false;
            idleTimeCounter = idleTime;
            invincible = false;
        }
        if (activeTimeCounter < 0 && idleTimeCounter < 0 && !angryMode)
        {
            ChoosePosition();
            anim.SetTrigger("Appear");
            angryMode = true;
            activeTimeCounter = activeTime;
            invincible = false;
        }

        FlipController();
    }

    private void FlipController()
    {
        if (player == null)
            return;

        if (facingDir == -1 && transform.position.x < player.transform.position.x)
        {
            Flip();
        }
        else if (facingDir == 1 && transform.position.x > player.transform.position.x)
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
        if (angryMode)
        {
            base.OnTriggerStay2D(collision);

        }
    }
}
