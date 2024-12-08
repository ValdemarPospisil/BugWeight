using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Crimson Vengeance")]
public class CrimsonVengeancePowerUp : PowerUp
{
    public GameObject bloodExplosionPrefab; // The blood explosion prefab to spawn
    private bool bloodRebirthActive;
    private int extraLivesOnTierIV;

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.OnFatalDamage += HandleFatalDamage;
        }
    }

    public override void Deactivate(GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.OnFatalDamage -= HandleFatalDamage;
        }
    }

    private void HandleFatalDamage(Vector3 position)
    {
        if (bloodExplosionPrefab != null)
        {
            GameObject explosion = Instantiate(bloodExplosionPrefab, position, Quaternion.identity);
            BloodExplosion explosionScript = explosion.GetComponent<BloodExplosion>();
            if (explosionScript != null)
            {
                var tier = tiers[currentTier - 1];
                explosionScript.SetUpExplosion(tier.damage, tier.speed, tier.duration);
            }
        }

        if (bloodRebirthActive && currentTier == maxTier)
        {
            PlayerManager playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.AddExtraLives(1, 50); // Add extra life on tier IV
            }
        }
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tiers.Count)
        {
            var tier = tiers[currentTier - 1];
            bloodRebirthActive = tier.damage > 0; // Using damage to check if Blood Rebirth is active
            extraLivesOnTierIV = Mathf.RoundToInt(tier.speed); // Using speed as extra lives on tier IV
        }
    }
}