using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/Crimson Aura")]
public class CrimsonAuraPowerUp : PowerUp
{
    public GameObject auraPrefab; // The aura effect prefab

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            var tier = tiers[currentTier - 1];

            GameObject aura = Instantiate(auraPrefab, playerManager.transform.position, Quaternion.identity);
            aura.transform.SetParent(playerManager.transform);

            var auraDamage = aura.GetComponent<CrimsonAuraDamage>();
            if (auraDamage != null)
            {
                auraDamage.Initialize(this);
            }
        }
    }

    public override void Deactivate(GameObject player)
    {
        // No deactivation needed for this power-up
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