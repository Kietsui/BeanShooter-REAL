using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;  // This should be a child of the Player GameObject
    public Transform catModel;     // Reference to the cat model

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Align the camera rotation with the cat's initial rotation
        yRotation = orientation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Calculate rotations
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotations to the camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        // Rotate the orientation object horizontally (this drives the cat's movement direction)
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Rotate the cat model to match the orientation
        catModel.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
