using UnityEngine;

public class ProjectilePowerUp : PowerUp
{
    public GameObject projectilePrefab; // The projectile to spawn
    public Transform spawnPoint; // Where to spawn the projectile
    public float shootInterval = 1f; // Time between shots

    private bool isActive = false;
    private float shootTimer = 0f;

    private void Update()
    {
        if (isActive)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                ShootProjectile();
                shootTimer = 0f;
            }
        }
    }

    public override void Activate(GameObject player)
    {
        isActive = true;
    }

    public override void Deactivate(GameObject player)
    {
        isActive = false;
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null && spawnPoint != null)
        {
            Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
