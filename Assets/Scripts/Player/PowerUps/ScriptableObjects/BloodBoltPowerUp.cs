using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Blood Bolt")]
public class BloodBoltPowerUp : PowerUp
{
    public string projectileName; // The projectile to spawn

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
        if (playerShooting != null)
        {
            playerShooting.StartShooting(this);
        }
    }

    public override void Deactivate(GameObject player)
    {
        PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
        if (playerShooting != null)
        {
            playerShooting.StopShooting();
        }
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tiers.Count)
        {
            var tier = tiers[currentTier - 1];
            // No need to store shootInterval and speed separately
        }
    }
}