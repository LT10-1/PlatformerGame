using UnityEngine;

public class Trap : MonoBehaviour
{
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {

        Player playerCollider = collision.GetComponent<Player>();
        if (playerCollider != null)
        {
            if (!playerCollider.isRoll)
                playerCollider.PlayerHit();

        }
    }
}
