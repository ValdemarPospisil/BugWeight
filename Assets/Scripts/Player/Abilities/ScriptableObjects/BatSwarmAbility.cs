using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Swarm", menuName = "SpecialAbilities/Bat Swarm")]
public class BatSwarmAbility : SpecialAbility
{

    
    [SerializeField] private float BatSpeed = 5f;

    private PlayerController playerController;

    public override void Activate()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        playerController.BatSwarm(damage, duration, BatSpeed);
    }

    protected override void UpdateProperties()
    {
        if (currentTier < maxTier)
        {
            damage += damage * percentageIncrease;
            cooldown -= cooldown * percentageIncrease;
            duration += duration * percentageIncrease;
            BatSpeed += BatSpeed * percentageIncrease;
        }
    }
}