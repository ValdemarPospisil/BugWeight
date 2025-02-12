using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Swarm", menuName = "SpecialAbilities/Bat Swarm")]
public class BatSwarmAbility : SpecialAbility
{

    
    private float damage;
    private float duration;
    private float BatSpeed;


    private PlayerController playerController;

    public override void Activate()
    {
        UpdateProperties();
        playerController = ServiceLocator.GetService<PlayerController>();

        playerController.BatSwarm(damage, duration, BatSpeed);
    }

    protected override void UpdateProperties()
    {
        if (currentTier < maxTier)
        {
            var tier = tierVariables[currentTier - 1];
            damage = tier.damage;
            duration = tier.duration;
            BatSpeed = tier.varFloat;
            cooldown = tier.cooldown;
        }
    }
}