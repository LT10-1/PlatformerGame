using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Spiked_Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 pushDir;

    void Start()
    {
        
        rb.AddForce(pushDir, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
