using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood Clone", menuName = "SpecialAbilities/Blood Clone")]
public class BloodCloneAbility : SpecialAbility
{
    [SerializeField] private GameObject bloodClonePrefab;
    [SerializeField] private float explosionRadius = 5f;
    private float damage;
    private float duration;
    private float healthOfTheWall;

    public override void Activate()
    {
        UpdateProperties();
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
            var tier = tierVariables[currentTier - 1];
            damage = tier.damage;
            duration = tier.duration;
            healthOfTheWall = tier.varFloat;
            cooldown = tier.cooldown;
            
        }
    }
}