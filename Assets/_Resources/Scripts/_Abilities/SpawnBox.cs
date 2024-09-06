using UnityEngine;
using Mirror;
using JetBrains.Annotations;

public class SpawnBox : NetworkBehaviour
{
    public GameObject boxPrefab; // The box prefab to spawn
    public float spawnHeight = 5f; // The height above the hit point to spawn the box
    public KeyCode spawnKey = KeyCode.E; // The key to press for spawning the box
    public float maxRayDistance = 100f; // Maximum distance for the raycast
    public int abilityDmg = 100;

    void Update()
    {
        if (!isLocalPlayer) return; // Ensure that only the local player can spawn boxes

        if (Input.GetKeyDown(spawnKey))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from the camera to the crosshair
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxRayDistance))
            {
                Vector3 spawnPosition = hit.point + Vector3.up * spawnHeight; // Calculate the spawn position above the hit point
                CmdSpawnBox(spawnPosition);
            }
        }
    }

    

    [Command]
    void CmdSpawnBox(Vector3 spawnPosition)
    {
        // Spawn the box on the server
        GameObject box = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
        NetworkServer.Spawn(box);
    }
}
