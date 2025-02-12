using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Swarm", menuName = "SpecialAbilities/Bat Swarm")]
public class BatSwarmAbility : SpecialAbility
{

    
    [SerializeField] private float BatSpeed = 5f;

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
            damage = tierVariables[currentTier].damage;
            duration = tierVariables[currentTier].duration;
            BatSpeed = tierVariables[currentTier].varFloat;
        }
    }
}