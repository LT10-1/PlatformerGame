using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField] private Transform resPoint;

    private void Awake()
    {
        PlayerManager.instance.respawnPoint = resPoint.transform;
        PlayerManager.instance.PlayerRespawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null) 
        {
            if(collision.transform.position.x<transform.position.x)
            GetComponent<Animator>().SetTrigger("Touch");    
        }
    }
}
