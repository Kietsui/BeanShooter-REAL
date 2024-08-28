using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletDmg = 40; // Damage the bullet will do

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player GameObject
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            // Check if the component exists
            if (playerHealth != null)
            {
                // Call the TakeDamage method
                playerHealth.TakeDamage(bulletDmg);
            }
            
            // Destroy the bullet after hitting the player
            Destroy(gameObject);
        }
    }
}
