using UnityEngine;

public class EyeAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public EyeDetection detection;
    public Transform firePoint;
    public Transform player;

    public float fireRate = 0.5f; // time between projectiles

    private float fireTimer = 0f;

    void Update()
    {
        if (detection.canSeePlayer)
        {
            // Count down timer
            fireTimer -= Time.deltaTime;

            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireRate;
            }
        }
        else
        {
            // Player hidden → reset timer so next shot is immediate when seen
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            proj.GetComponent<Projectile>().Initialize(player, detection);
        }
    }
}