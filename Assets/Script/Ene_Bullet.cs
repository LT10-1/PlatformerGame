using UnityEngine;

public class Ene_Bullet : Enemy
{

    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    [SerializeField] float gravity;
    [SerializeField] Vector2 velocity;


    protected override void Start()
    {
        base.Start();
        gravity =15f;
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        rb.velocity = new Vector2(xSpeed, -velocity.y);

        
    }

    

    public void SetupSpeed(float x, float y)
    {
        xSpeed = x;
        ySpeed = y;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {

        base.OnTriggerStay2D(collision);
        Destroy(gameObject);
    }
}
