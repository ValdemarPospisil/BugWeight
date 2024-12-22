using UnityEngine;

[CreateAssetMenu(fileName = "Wolf Form", menuName = "SpecialAbilities/Wolf Form")]
public class WolfFormAbility : SpecialAbility
{
    [SerializeField] private float hpBoost = 50f; 
    [SerializeField] private GameObject wolfPrefab;     
    [SerializeField] private float freezeRadius = 5; 
    [SerializeField] private float freezeDuration = 10f;  

    public override void Activate()
    {
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
            hpBoost += hpBoost * percentageIncrease;
            duration += duration * percentageIncrease;
            cooldown -= cooldown * percentageIncrease;
            damage += damage * percentageIncrease;
            freezeDuration += freezeDuration * percentageIncrease;
            freezeRadius += freezeRadius * percentageIncrease;
        }
    }
}
