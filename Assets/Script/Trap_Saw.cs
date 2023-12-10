using UnityEngine;

public class Trap_Saw : Trap
{
    private Animator anim;

    [SerializeField] private bool isWorking;

    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed;
    [SerializeField] private float cooldown;

    private int movePointIndex;
    private float cooldownTimer;


    private void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = movePoint[0].position;
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        bool isWorking = cooldownTimer < 0;
        anim.SetBool("isWorking", isWorking);

        if (isWorking)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        }

        if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < .01f)
        {
            Flip();
            cooldownTimer = cooldown;
            movePointIndex++;
            if (movePointIndex >= movePoint.Length)
            {
                movePointIndex = 0;
            }
        }

    }

    private void Flip()
    {
        transform.localScale = new Vector3(1, transform.localScale.y * -1);
    }    

    
}
