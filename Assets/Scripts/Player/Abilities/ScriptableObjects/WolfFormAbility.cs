using UnityEngine;

[CreateAssetMenu(fileName = "Wolf Form", menuName = "SpecialAbilities/Wolf Form")]
public class WolfFormAbility : SpecialAbility
{
    [SerializeField] private float hpBoost = 50f; 
    [SerializeField] private float wolfDuration = 10f;  
    [SerializeField] private GameObject wolfPrefab;     

    public override void Activate()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (player != null)
        {
            playerController.StartCoroutine(playerController.TransformToWolf(hpBoost, wolfDuration, wolfPrefab));
        }
    }

    protected override void UpdateProperties()
    {
        if (currentTier <= maxTier)
        {
            hpBoost += hpBoost * percentageIncrease;
            wolfDuration += wolfDuration * percentageIncrease;
            cooldown -= cooldown * percentageIncrease;
        }
    }
}
