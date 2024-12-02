using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Shadow Step", menuName = "SpecialAbilities/Shadow Step")]
public class ShadowStepAbility : SpecialAbility
{
    [SerializeField] private float teleportDistance = 5f;
    [SerializeField] private float shadowExplosionDelay = 1f;
    [SerializeField] private float explosionDamage = 50f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private GameObject shadowExplosionEffect;

    public override void Activate()
    {
        var playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerController.StartCoroutine(playerController.ShadowStep(teleportDistance, shadowExplosionDelay, explosionDamage, explosionRadius, shadowPrefab, shadowExplosionEffect));
    }

}