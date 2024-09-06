using System.Collections;
using UnityEngine;
using Mirror;

public class GunShoot : NetworkBehaviour
{
    public Transform bulletSpawnPoint;  // Reference to the bullet spawn point
    public GameObject bulletPrefab;     // Reference to the bullet prefab
    public float bulletSpeed = 10f;     // Speed of the bullet
    public KeyCode key;                 // The key to trigger shooting

    [Tooltip("Fire rate in rounds per second (RPS)")]
    public float roundsPerSecond = 10f; // Fire rate in rounds per second (RPS)

    private bool isFiring = false;      // Flag to track if the gun is currently firing
    private float fireRate;             // Time between shots (calculated from RPS)

    private AudioSource source;
    [SerializeField] private AudioClip gunShot;

    [SerializeField] private GameObject muzzleFlashPrefab;
    private GameObject currentMuzzleFlash;
    public Camera playerCamera; 
    void Start()
    {
        // Calculate fireRate based on rounds per second
        fireRate = 1f / roundsPerSecond;
        source = GetComponent<AudioSource>();
        playerCamera = FindAnyObjectByType<Camera>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetKeyDown(key))
        {
            StartFiring();
        }

        if (Input.GetKeyUp(key))
        {
            StopFiring();
        }
    }

    void StartFiring()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(FireCoroutine());
        }
    }

    void StopFiring()
    {
        isFiring = false;
        StopCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        while (isFiring)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate); // Wait based on the calculated fire rate
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null && gunShot != null)
        {
            source.PlayOneShot(gunShot);

            // Show the muzzle flash
            if (muzzleFlashPrefab != null)
            {
                if (currentMuzzleFlash != null)
                {
                    Destroy(currentMuzzleFlash);
                }
                currentMuzzleFlash = Instantiate(muzzleFlashPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Destroy(currentMuzzleFlash, 0.1f); // Destroy after a short delay
            }

            // Calculate the bullet direction using a raycast from the center of the screen
            Vector3 bulletDirection = GetShootDirection();

            // Call command to handle bullet instantiation on the server
            CmdShoot(bulletDirection);
        }
        else
        {
            Debug.LogError("BulletPrefab or BulletSpawnPoint is not assigned.");
        }
    }

    Vector3 GetShootDirection()
{
    Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Ray from the center of the screen
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit))
    {
        // If we hit something, return the direction to the hit point
        return (hit.point - bulletSpawnPoint.position).normalized;
    }
    else
    {
        // If nothing was hit, shoot straight ahead
        return playerCamera.transform.forward;
    }
}

[Command]
void CmdShoot(Vector3 bulletDirection)
{
    // Instantiate the bullet on the server
    GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
    NetworkServer.Spawn(bullet);

    // Set the bullet's velocity based on the calculated direction
    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
    if (bulletRb != null)
    {
        bulletRb.velocity = bulletDirection * bulletSpeed;
    }
    else
    {
        Debug.LogError("The bulletPrefab does not have a Rigidbody component.");
    }

    // Ignore collisions between the bullet and the player (or the shooter)
    Collider bulletCollider = bullet.GetComponent<Collider>();
    Collider playerCollider = GetComponent<Collider>(); // Assuming the script is on the player
    if (bulletCollider != null && playerCollider != null)
    {
        Physics.IgnoreCollision(bulletCollider, playerCollider);
    }

    // Optionally handle muzzle flash instantiation on the server
    if (muzzleFlashPrefab != null)
    {
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        NetworkServer.Spawn(muzzleFlash);
        Destroy(muzzleFlash, 0.1f); // Destroy after a short delay
    }
}

}
