using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Crimson Vengeance")]
public class CrimsonVengeancePowerUp : PowerUp
{
    public GameObject bloodExplosionPrefab; // The blood explosion prefab to spawn
    private int extraLivesOnTierIV;

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        
        Enemy.explosionChance = tiers[currentTier - 1].duration;
        Enemy.explosionRadius = tiers[currentTier - 1].speed;
        Enemy.explosionDamage = tiers[currentTier - 1].damage;
        Enemy.explodeOnDeath = true;

    }
    

    public override void Deactivate()
    {
        Debug.Log("Deactivate Crimson Vengeance");
        PlayerManager.explodeOnDeath = false;
        Enemy.explodeOnDeath = false;
    }
/*
    private void HandleFatalDamage(Vector3 position)
    {
        if (currentTier == maxTier)
        {
            
            PlayerManager.Instance.AddExtraLives(1, 50); // Add extra life on tier IV
            

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
        }
    }
*/
    protected override void UpdateProperties()
    {
        if (currentTier < tiers.Count)
        {
            var tier = tiers[currentTier - 1];
        }

         if (currentTier == maxTier)
        {
            Debug.Log("Activate Crimson Vengeance Tire IV");
            PlayerManager.Instance.AddExtraLives(1, 0.05f); // Add extra life on tier IV

            PlayerManager.explosionRadius = tiers[currentTier - 1].speed;
            PlayerManager.explosionDamage = tiers[currentTier - 1].damage;
            PlayerManager.explodeOnDeath = true;

        }
    }
}