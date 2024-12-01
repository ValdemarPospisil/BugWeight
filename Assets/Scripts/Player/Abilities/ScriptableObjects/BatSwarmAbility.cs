using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Bat Swarm", menuName = "SpecialAbilities/Bat Swarm")]
public class BatSwarmAbility : SpecialAbility
{
    [SerializeField] private float damage = 5f;
    [SerializeField] private float BatsDuration = 5f;
    [SerializeField] private float BatSpeed = 5f;

    private PlayerController playerController;

    public override void Activate()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        playerController.BatSwarm(damage, BatsDuration, BatSpeed);
    }
}
