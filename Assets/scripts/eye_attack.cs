using UnityEngine;

public class EyeAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public EyeDetection detection;
    public Transform firePoint;
    public Transform player;

    public float fireRate = 0.5f; // seconds between shots

    float fireTimer;

    void Update()
    {
        if (detection == null)
            return;

        if (detection.canSeePlayer)
        {
            fireTimer -= Time.deltaTime;

            if (fireTimer <= 0f)
            {
                Shoot();
                fireTimer = fireRate;
            }
        }
        else
        {
            // reset so the first shot fires immediately when the player is seen again
            fireTimer = 0f;
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = proj.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Initialize(player, detection);
        }
    }
}
