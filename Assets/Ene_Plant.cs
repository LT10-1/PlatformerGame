using UnityEngine;

public class Ene_Plant : Enemy
{
    protected override void Start()
    {
        base.Start();
    }


    // Update is called once per frame
    void Update()
    {
        CollisionCheck();
        idleTimeCounter -= Time.deltaTime;

        bool playerDetected = playerDetection.collider.GetComponent<Player>() != null;
        if (idleTimeCounter < 0 && playerDetected)
        {
            idleTimeCounter = idleTime;
            anim.SetTrigger("Attack");
        }

    }

    private void AttackEvent()
    {
        Debug.Log("Attack!" + playerDetection.collider.name);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
