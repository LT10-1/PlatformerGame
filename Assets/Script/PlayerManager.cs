using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Transform respawnPoint;
    [SerializeField] private GameObject playerprefab;
    public GameObject currentPlayer;

    private void Awake()
    {
        instance = this;
        
    }

    public void PlayerRespawn()
    {
        if (currentPlayer == null)
        {
            currentPlayer = Instantiate(playerprefab, respawnPoint.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
