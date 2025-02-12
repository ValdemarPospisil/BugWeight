using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Vampiric Hunger")]
public class VampiricHungerPowerUp : PowerUp
{
    public GameObject bloodOrbPrefab;
    public float spawnChance;
    public float healAmount;

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        Enemy.OnEnemyKilled += HandleEnemyKilled;
    }

    public override void Deactivate()
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
        if (currentTier < tierVariables.Count)
        {
            var tier = tierVariables[currentTier - 1];
            spawnChance = tier.variable;
            healAmount = tier.damage;
        }
    }
}