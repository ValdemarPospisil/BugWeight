using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Summon Wolf")]
public class SummonWolfPowerUp : PowerUp
{
    public GameObject wolfPrefab; // The wolf prefab to spawn
    private float damage;
    private float speed;
    private float duration;

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        Vector3 spawnPosition = player.transform.position + new Vector3(1, 0, 0); // Spawn next to the player
        GameObject wolf = Instantiate(wolfPrefab, spawnPosition, Quaternion.identity);
        WolfBehavior wolfBehavior = wolf.GetComponent<WolfBehavior>();
        wolfBehavior.Initialize(player, damage, speed, duration);
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
            damage = tier.damage;
            speed = tier.speed;
            duration = tier.duration;
        }
        // Update properties based on the current tier if needed
    }
}