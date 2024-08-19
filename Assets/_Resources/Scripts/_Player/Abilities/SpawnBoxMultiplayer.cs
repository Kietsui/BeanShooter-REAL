using UnityEngine;
using Alteruna;

public class SpawnBox : MonoBehaviour
{
    public GameObject boxPrefab; // The box prefab to spawn
    public float spawnHeight = 5f; // The height above the hit point to spawn the box
    public KeyCode spawnKey = KeyCode.E; // The key to press for spawning the box
    public float maxRayDistance = 100f; // Maximum distance for the raycast

    private Multiplayer multiplayerManager; // Reference to your MultiplayerManager script

    void Start()
    {
        // Find the MultiplayerManager instance in the scene
        multiplayerManager = FindObjectOfType<Multiplayer>();

        // Check if the multiplayerManager instance was found
        if (multiplayerManager == null)
        {
            Debug.LogError("MultiplayerManager instance not found in the scene. Make sure a GameObject with the MultiplayerManager script exists.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(spawnKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from the camera to the crosshair
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                Vector3 spawnPosition = hit.point + Vector3.up * spawnHeight; // Calculate the spawn position above the hit point
                SpawnBoxRPC(spawnPosition);
            }
        }
    }

    private void SpawnBoxRPC(Vector3 spawnPosition)
    {
        // Assuming MultiplayerManager has a method for spawning networked objects
        //multiplayerManager.SpawnNetworkedPrefab(boxPrefab, spawnPosition, Quaternion.identity);
    }
}
