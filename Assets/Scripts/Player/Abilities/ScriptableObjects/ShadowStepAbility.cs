using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Shadow Step", menuName = "SpecialAbilities/Shadow Step")]
public class ShadowStepAbility : SpecialAbility
{
    private float damage;
    private float duration;
    private float teleportDistance;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private GameObject shadowExplosionEffect;

    public override void Activate()
    {
        UpdateProperties();
        var playerController = ServiceLocator.GetService<PlayerController>();
        playerController.StartCoroutine(playerController.ShadowStep(teleportDistance, duration, damage, explosionRadius, shadowPrefab, shadowExplosionEffect));
    }
    protected override void UpdateProperties()
    {
        if (currentTier < maxTier)
        {
            var tier = tierVariables[currentTier - 1];
            damage = tier.damage;
            duration = tier.duration;
            teleportDistance = tier.varFloat;
            cooldown = tier.cooldown;
        }
    }

}