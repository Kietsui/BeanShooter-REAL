using UnityEngine;

public class RotateGun : MonoBehaviour
{
    private Transform playerCamera;  // Reference to the player's camera

    // Method to set the player camera reference
    public void SetPlayerCamera(Transform cameraTransform)
    {
        playerCamera = cameraTransform;
    }

    private void Update()
    {
        // Ensure we only run this if we have the player camera set
        if (playerCamera == null) return;

        // Rotate the gun's parent to match the camera's rotation
        transform.rotation = Quaternion.Euler(playerCamera.eulerAngles.x, playerCamera.eulerAngles.y, 0);
    }
}
