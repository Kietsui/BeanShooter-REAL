using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public HealthBar healthBar;  // Reference to the health bar
    public Transform playerCamera; // Reference to the player's camera

    [SyncVar(hook = nameof(OnHpChanged))] // Hook to update health bar when playerHp changes
    private int playerHp;
    [SyncVar] public int playerMaxHp = 250;

    [SerializeField] private float hideAngleThreshold = 70f;  // Threshold for hiding health bar when looking up

    private void Start()
    {
        playerHp = playerMaxHp;
        Debug.Log($"Player HP: {playerHp}");

        // Hide the health bar for the local player (so they don't see their own health bar)
        if (isLocalPlayer)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Only check angle for other players' health bars
        if (!isLocalPlayer)
        {
            CheckHealthBarVisibility();
        }
    }

    // This method is called on the server to apply damage to the player
    public void TakeDamage(int amount)
    {
        if (!isServer) return;  // Only execute this on the server

        playerHp -= amount; // Subtract health first
        Debug.Log($"Player took damage, current HP: {playerHp}");

        // Optionally, handle player death
        if (playerHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    // This method is called on all clients when playerHp changes
    private void OnHpChanged(int oldHp, int newHp)
    {
        Debug.Log($"Player HP changed from {oldHp} to {newHp}");

        // Update the health bar
        healthBar.UpdateHealthbar((float)newHp / (float)playerMaxHp);
    }

    // Check if the health bar should be visible based on player's view angle
    private void CheckHealthBarVisibility()
    {
        if (playerCamera == null) return;

        // Get the direction from the player to the camera
        Vector3 toCamera = playerCamera.position - transform.position;

        // Calculate the angle between the player's forward direction and the direction to the camera
        float angle = Vector3.Angle(transform.forward, toCamera);

        // Hide the health bar if the player is looking up beyond the threshold
        if (angle > hideAngleThreshold)
        {
            healthBar.gameObject.SetActive(false);
        }
        else
        {
            healthBar.gameObject.SetActive(true);
        }
    }
}
