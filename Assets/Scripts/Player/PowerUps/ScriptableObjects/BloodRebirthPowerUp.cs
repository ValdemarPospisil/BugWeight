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
        if (currentTier < tierVariables.Count)
        {
            var tier = tierVariables[currentTier - 1];
            healthPercentage = tier.damage; // Using damage as health percentage
            extraLives =  tier.varInt; // Using speed as extra lives
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.AddExtraLives(extraLives, healthPercentage);
        }
    }
}