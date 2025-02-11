using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Blood Rebirth")]
public class BloodRebirthPowerUp : PowerUp
{
    private float healthPercentage; // Percentage of max health to restore on resurrection
    private int extraLives; // Number of extra lives granted

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        
    }

    public override void Deactivate()
    {
        // No deactivation needed for this power-up
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tiers.Count)
        {
            var tier = tiers[currentTier - 1];
            healthPercentage = tier.damage; // Using damage as health percentage
            extraLives = Mathf.RoundToInt(tier.speed); // Using speed as extra lives
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.AddExtraLives(extraLives, healthPercentage);
        }
    }
}