using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Vampiric Hunger")]
public class VampiricHungerPowerUp : PowerUp
{
    public GameObject bloodOrbPrefab; // The blood orb prefab to spawn
    public float spawnChance; // Chance to spawn a blood orb on enemy kill
    public float healAmount; // Amount of health restored by the blood orb

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        Enemy.OnEnemyKilled += HandleEnemyKilled;
    }

    public override void Deactivate(GameObject player)
    {
        Enemy.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void HandleEnemyKilled(Vector3 position)
    {
        if (bloodOrbPrefab == null)
        {
            Debug.LogError("BloodOrbPrefab is not assigned.");
            return;
        }

        if (Random.value <= spawnChance)
        {
            GameObject bloodOrb = Instantiate(bloodOrbPrefab, position, Quaternion.identity);
            BloodOrb orbScript = bloodOrb.GetComponentInChildren<BloodOrb>();
            if (orbScript != null)
            {
                orbScript.SetHealAmount(healAmount);
            }
            else
            {
                Debug.LogError("BloodOrb script not found on BloodOrb prefab.");
            }
        }
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tiers.Count)
        {
            var tier = tiers[currentTier - 1];
            spawnChance = tier.speed; // Using speed as spawn chance
            healAmount = tier.damage; // Using damage as heal amount
        }
    }
}