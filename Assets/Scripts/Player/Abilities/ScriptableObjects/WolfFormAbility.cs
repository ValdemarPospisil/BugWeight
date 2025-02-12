using UnityEngine;

[CreateAssetMenu(fileName = "Wolf Form", menuName = "SpecialAbilities/Wolf Form")]
public class WolfFormAbility : SpecialAbility
{
    private float damage;
    private float duration;
    private float hpBoost;
    
    [SerializeField] private GameObject wolfPrefab;     
    [SerializeField] private float freezeRadius = 5; 
    [SerializeField] private float freezeDuration = 2;

    public override void Activate()
    {
        UpdateProperties();
        var player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (player != null)
        {
            playerController.StartCoroutine(playerController.TransformToWolf(hpBoost, duration, wolfPrefab, damage, freezeRadius, freezeDuration));
        }
    }

    protected override void UpdateProperties()
    {
        if (currentTier <= maxTier)
        {
            var tier = tierVariables[currentTier - 1];
            damage = tier.damage;
            duration = tier.duration;
            hpBoost = tier.varFloat;
            cooldown = tier.cooldown;
        }
    }
}
