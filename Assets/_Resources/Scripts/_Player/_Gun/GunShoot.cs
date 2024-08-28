using System.Collections;
using UnityEngine;
using Mirror;

public class GunShoot : NetworkBehaviour
{
    public Transform bulletSpawnPoint; // Reference to the bullet spawn point
    public GameObject bulletPrefab;    // Reference to the bullet prefab
    public float bulletSpeed = 10f;    // Speed of the bullet
    public KeyCode key;                // The key to trigger shooting

    [Tooltip("Fire rate in rounds per second (RPS)")]
    public float roundsPerSecond = 10f; // Fire rate in rounds per second (RPS)

    private bool isFiring = false;     // Flag to track if the gun is currently firing

    private float fireRate;            // Time between shots (calculated from RPS)

    private AudioSource source;
    [SerializeField]
    private AudioClip gunShot;

    [SerializeField]
    private GameObject muzzleFlashPrefab; // Muzzle flash prefab

    private GameObject currentMuzzleFlash; // Current muzzle flash instance

    void Start()
    {
        // Calculate fireRate based on rounds per second
        fireRate = 1f / roundsPerSecond;

        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isLocalPlayer) return; // Ensure that only the local player can control their gun

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

            // Instantiate and show the muzzle flash
            if (muzzleFlashPrefab != null)
            {
                if (currentMuzzleFlash != null)
                {
                    Destroy(currentMuzzleFlash);
                }
                currentMuzzleFlash = Instantiate(muzzleFlashPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                Destroy(currentMuzzleFlash, 0.1f); // Destroy after a short delay
            }

            // Instantiate the bullet at the spawn point's position and rotation
            CmdShoot(); // Call command to handle bullet instantiation on the server
        }
        else
        {
            Debug.LogError("BulletPrefab or BulletSpawnPoint is not assigned.");
        }
    }

    [Command]
    void CmdShoot()
    {
        // Instantiate the bullet on the server
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        NetworkServer.Spawn(bullet);

        // Set the bullet's velocity
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
        else
        {
            Debug.LogError("The bulletPrefab does not have a Rigidbody component.");
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
