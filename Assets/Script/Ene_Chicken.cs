using UnityEngine;

public class Ene_Chicken : Enemy
{
   
    

    protected override void Start()
    {
        base.Start();
        
    }

    private void Update()
    {
        if(isDead) return;
        WalkAround();


        CollisionCheck();
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
