using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood Clone", menuName = "SpecialAbilities/Blood Clone")]
public class BloodCloneAbility : SpecialAbility
{
    [SerializeField] private GameObject bloodClonePrefab;
    [SerializeField] private float explosionRadius = 5f;

    public override void Activate()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.StartCoroutine(playerController.UseCloneAbility(bloodClonePrefab, duration, damage, explosionRadius));
            }
        }
    }
    protected override void UpdateProperties()
    {
        if (currentTier < maxTier)
        {
            damage += damage * percentageIncrease;
            cooldown -= cooldown * percentageIncrease;
            duration += duration * percentageIncrease;
            
        }
    }
}