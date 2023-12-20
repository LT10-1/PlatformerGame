using UnityEngine;

public class Ene_Plant : Enemy
{
    [Header("Plant spesifics")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform buttletOrigin;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float flipTime = 2;
    [SerializeField] private float flipTimeCounter;

    protected override void Start()
    {
        base.Start();

    }


    // Update is called once per frame
    void Update()
    {
        CollisionCheck();
        idleTimeCounter -= Time.deltaTime;
        flipTimeCounter -= Time.deltaTime;

        
        if (idleTimeCounter < 0/* && playerDetection*/)
        {
            idleTimeCounter = idleTime;
            anim.SetTrigger("Attack");
        }
        if(flipTimeCounter < 0)
        {
            flipTimeCounter = flipTime;
            Flip();
        }
    }

    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, buttletOrigin.transform.position, buttletOrigin.transform.rotation);

        newBullet.GetComponent<Ene_Bullet>().SetupSpeed(bulletSpeed * facingDir, 0f);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }


}
