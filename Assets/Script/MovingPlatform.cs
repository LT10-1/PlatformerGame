using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform;
    public GameObject[] waypoints;
    public float speed = 2.0f;
    private int currentWaypoint = 0;
    public float platformSizeX;
    public float platformSizeY;
    public LayerMask playerLayer;
    


   
    void Update()
    {
        if (waypoints.Length == 0) return;

        if (transform.position == waypoints[currentWaypoint].transform.position)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, speed * Time.deltaTime);

        Collider2D hit = Physics2D.OverlapBox(transform.position, new Vector2(platformSizeX, platformSizeY), 0, playerLayer);
        if (hit != null)
        {
            if (hit.gameObject.tag == "Player" || hit.gameObject.tag == "Enemy")
            {
                hit.transform.parent = platform.transform;
            }
        }
        else
        {
            foreach (Transform child in platform.transform)
            {
                if (child.gameObject.tag == "Player" || child.gameObject.tag == "Enemy")
                {
                    child.transform.parent = null;
                }
            }
        }
    }

   
    

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(platformSizeX, platformSizeY, 0));
    }
}
