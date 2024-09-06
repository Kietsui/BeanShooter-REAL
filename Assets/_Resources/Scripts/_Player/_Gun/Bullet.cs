using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private int bulletDmg = 40;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isServer)
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(bulletDmg);
                }
                else
                {
                    Debug.Log("PlayerHealth is null");
                }
            }

            // Destroy the bullet on all clients
            NetworkServer.Destroy(gameObject); // This is server-side and works across clients
        }
        else if (collision.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
    
}
