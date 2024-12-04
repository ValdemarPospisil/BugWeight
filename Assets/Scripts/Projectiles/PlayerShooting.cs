using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint; // Where projectiles will spawn

    private BloodBoltPowerUp activePowerUp;
    private float shootTimer;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (activePowerUp != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= activePowerUp.tiers[activePowerUp.currentTier - 1].duration)
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

        // Define directions based on the tier
        Vector2[] directions = GetDirectionsForTier(activePowerUp.currentTier);

        // Spawn a projectile in each direction
        foreach (Vector2 direction in directions)
        {
            // Instantiate a new GameObject for each projectile
            GameObject projectile = Instantiate(activePowerUp.projectilePrefab, firePoint.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            // Initialize the projectile with the properties from the power-up
            var tier = activePowerUp.tiers[activePowerUp.currentTier - 1];
            projectileScript.Initialize(direction, tier.speed, tier.damage);
        }
    }

    private Vector2[] GetDirectionsForTier(int tier)
    {
        // Define directions based on the tier
        switch (tier)
        {
            case 1:
                return new Vector2[] { playerController.lastDirection };
            case 2:
                return new Vector2[] { playerController.lastDirection, -playerController.lastDirection };
            case 3:
                return new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
            case 4:
                return new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right,
                    new Vector2(1, 1).normalized, new Vector2(1, -1).normalized,
                    new Vector2(-1, 1).normalized, new Vector2(-1, -1).normalized };
            default:
                return new Vector2[] { playerController.lastDirection };
        }
    }
}