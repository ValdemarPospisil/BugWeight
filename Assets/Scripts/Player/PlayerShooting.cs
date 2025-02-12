using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint; // Where projectiles will spawn

    private BloodBoltPowerUp activePowerUp;
    private float shootTimer;
    private float shootCooldown = 1f;
    private PlayerController playerController;
    private ProjectileFactory projectileFactory;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        projectileFactory = ServiceLocator.GetService<ProjectileFactory>();
    }

    private void Update()
    {
        if (activePowerUp != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootCooldown)
            {
                ShootProjectiles();
                shootTimer = 0f;
            }
        }
    }

    public void StartShooting(BloodBoltPowerUp powerUp)
    {
        activePowerUp = powerUp;
        shootTimer = 0f;
    }

    public void StopShooting()
    {
        activePowerUp = null;
    }

    private void ShootProjectiles()
    {
        if (activePowerUp == null) return;

        Vector2[] directions = GetDirectionsForTier(activePowerUp.currentTier);

        foreach (Vector2 direction in directions)
        {
            if (activePowerUp.projectileName == null)
            {
                Debug.Log("Projectile name is null");
                return;
            }

            projectileFactory.SpawnProjectile(activePowerUp.projectileName, firePoint.position, direction);
        }
    }

    private Vector2[] GetDirectionsForTier(int tier)
    {
        // Use the player's current input direction instead of lastDirection
        Vector2 currentDirection = playerController.GetCurrentDirection();

        switch (tier)
        {
            case 1:
                return new Vector2[] { currentDirection };
            case 2:
                return new Vector2[] { currentDirection, -currentDirection };
            case 3:
                return new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            case 4:
                return new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right,
                    new Vector2(1, 1).normalized, new Vector2(1, -1).normalized,
                    new Vector2(-1, 1).normalized, new Vector2(-1, -1).normalized };
            default:
                return new Vector2[] { currentDirection };
        }
    }
}