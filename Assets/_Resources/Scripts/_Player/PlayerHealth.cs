using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int playerHp;
    public int playerMaxHp = 250;

    private void Start() 
    {
        playerHp = playerMaxHp;    
        Debug.Log($"Player HP: " + playerHp);
    }

    public void TakeDamage(int amount)
    {
        playerHp -= amount;
        Debug.Log($"Player took damage, current HP: {playerHp}");

        // Optionally, add code to handle player death if HP drops to 0 or below
    }
}
