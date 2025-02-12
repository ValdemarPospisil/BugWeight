using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Summon Wolf")]
public class SummonWolfPowerUp : PowerUp
{
    public GameObject wolfPrefab;
    private float damage;
    private float speed;
    private float duration;

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        Vector3 spawnPosition = player.transform.position + new Vector3(1, 0, 0);
        GameObject wolf = Instantiate(wolfPrefab, spawnPosition, Quaternion.identity);
        WolfBehavior wolfBehavior = wolf.GetComponent<WolfBehavior>();
        wolfBehavior.Initialize(player, damage, speed, duration);
    }

    public override void Deactivate()
    {
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tierVariables.Count)
        {
            var tier = tierVariables[currentTier - 1];
            damage = tier.damage;
            speed = tier.variable;
            duration = tier.variable * 2;
        }
    }
}