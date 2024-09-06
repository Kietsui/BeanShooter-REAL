using UnityEngine;
using Mirror;
using JetBrains.Annotations;
using System.Linq.Expressions;

public class abilityBox : NetworkBehaviour
{
    [SerializeField] private int abilityDmg = 100;

    public bool hasHitPlayer = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && hasHitPlayer == false)
        {
            if (isServer)
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(abilityDmg);
                    hasHitPlayer = true;
                    Debug.Log(hasHitPlayer);
                }
                else
                {
                    Debug.Log("PlayerHealth is null");
                }
            }

            // Destroy the bullet on all clients
            //NetworkServer.Destroy(gameObject); // This is server-side and works across clients
        }
    }
}
