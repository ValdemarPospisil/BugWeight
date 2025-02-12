using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood Clone", menuName = "SpecialAbilities/Blood Clone")]
public class BloodCloneAbility : SpecialAbility
{
    [SerializeField] private GameObject bloodClonePrefab;
    private float damage;
    private float duration;
    private float cloneHealth;

    public override void Activate()
    {
        UpdateProperties();
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.StartCoroutine(playerController.UseCloneAbility(bloodClonePrefab, duration, damage, cloneHealth));
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
            cloneHealth = tier.varFloat;
            cooldown = tier.cooldown;
            
        }
    }
}