using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerHp;
    public int playerMaxHp = 250;

    private void Start() 
    {
        playerHp = playerMaxHp;    
    }


    public void TakaDamage(int amount)
    {
        playerHp -= amount;
    }
}
