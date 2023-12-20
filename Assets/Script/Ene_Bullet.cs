using UnityEngine;

public class Ene_Bullet : Enemy
{

     private float xSpeed;
     private float ySpeed;



    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(xSpeed, ySpeed);
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
