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
        if (playerCamera == null) return;

        // Rotate the gun's parent to match the camera's rotation
        transform.rotation = Quaternion.Euler(playerCamera.eulerAngles.x, playerCamera.eulerAngles.y, 0);
    }
}
