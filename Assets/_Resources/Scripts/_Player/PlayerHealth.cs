using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SyncVar] private int playerHp;
    [SyncVar] public int playerMaxHp = 250;

    private void Start() 
    {
        playerHp = playerMaxHp;
        Debug.Log($"Player HP: " + playerHp);
    }

    // This method is called on the server to apply damage to the player
    public void TakeDamage(int amount)
    {
        if (!isServer) return;  // Only execute this on the server

        playerHp -= amount;
        Debug.Log($"Player took damage, current HP: {playerHp}");

        // Optionally, add code to handle player death if HP drops to 0 or below
        if (playerHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
