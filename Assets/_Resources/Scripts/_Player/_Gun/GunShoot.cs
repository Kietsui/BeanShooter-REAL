using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public Transform bulletSpawnPoint; // Reference to the bullet spawn point
    public GameObject bulletPrefab;    // Reference to the bullet prefab
    public float bulletSpeed = 10f;    // Speed of the bullet

    public KeyCode key;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            // Instantiate the bullet at the spawn point's position and rotation
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            // Ensure the bullet has a Rigidbody component
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                // Set the bullet's velocity to move forward relative to the spawn point
                bulletRb.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }
            else
            {
                Debug.LogError("The bulletPrefab does not have a Rigidbody component.");
            }
        }
        else
        {
            Debug.LogError("BulletPrefab or BulletSpawnPoint is not assigned.");
        }
    }
}
