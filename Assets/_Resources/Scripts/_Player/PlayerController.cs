using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [Header("Base setup")]
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 90f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    [SerializeField] private float cameraYOffset = 0.4f;
    [SerializeField] private RotateGun rotateGun;
    private Camera playerCamera;

    void Start()
    {
        // Check if this is the local player
        if (!isLocalPlayer)
        {
            return;
        }
        
        characterController = GetComponent<CharacterController>();
        playerCamera = Camera.main;

        // Set camera position and parent to the player
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, transform.position.z);
        playerCamera.transform.SetParent(transform);

        // Assign the local player's camera to the RotateGun script
        if (rotateGun != null)
        {
            rotateGun.SetPlayerCamera(playerCamera.transform);
        }

        // Locks cursor to the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // If the script isnt run by the local player, do nothing.
        if (!isLocalPlayer)
        {
            return;
        }

        bool isRunning = false;

        // Hardcoded sprint key
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Converts local direction to world space
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // If canMove == false; return 0 (no movement). If canMove == true and isRunning == true; return runningSpeed
        // If canMove == true and isRunning == false; return walkingSpeed
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        
        // Specifies that to use the Y axis.
        float movementDirectionY = moveDirection.y;
        
        // combines both movement vectors into a single variable that 
        // controls where the player should move (left/right, forward/backward).
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);


        // Controls the players jumping.
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            // Stores the players previous Y-axis movement from the last frame to ensure that when the player
            // isnt jumping your vertical movement isnt reset to zero every frame.
            moveDirection.y = movementDirectionY;
        }

        // If the player isnt grounded, reduce the vertical speed * deltaTime to simulate gravity.
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Uses CharacterControllers Move function to move the player based on the given input. 
        // Time.deltTime ensures smooth movement by applying to every frame.
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove && playerCamera != null)
        {
            // Gets the mouse up/down rotation.
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;

            // Locks the mouse rotation to not go past a certain point.
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Sets the cameras local rotation in relation to the parent object (The player).
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Rotates the player object based on the mouse rotation (left/right).
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
