using UnityEngine;

public class trampoline : MonoBehaviour
{
    [SerializeField] private float pushForce;
    [SerializeField] private bool canbeUsed = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Player>() != null && canbeUsed)
        {
            canbeUsed = false;
            GetComponent<Animator>().SetTrigger("active");
            collision.GetComponent<Player>().Push(pushForce);
        }



    }
    private void CanUseAgain() => canbeUsed = true;
    
}
