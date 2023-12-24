using UnityEngine;

public class Ene_Bullet : Enemy
{

    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;



    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(transform.position.x * 2f * facingDir, ySpeed);
        rb.AddForce(-transform.up * 15f);
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
