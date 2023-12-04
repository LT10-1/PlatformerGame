using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Player playerCollider = collision.GetComponent<Player>();
            if (playerCollider != null)
            {
                if(!playerCollider.isRoll)    
                     playerCollider.PlayerHit();
                

            }
        }

    }
}
