using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood Surge", menuName = "SpecialAbilities/Blood Surge")]
public class BloodSurgeAbility : SpecialAbility
{
    private float dashSpeed = 10f;
    private float dashDuration = 0.2f;
    private float mistDamage = 3f;
    [SerializeField] private GameObject bloodTrailPrefab;
    

    public override void Activate()
    {
        UpdateProperties();
        var player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (player != null)
        {
            playerController.StartCoroutine(playerController.BloodSurge(dashSpeed, dashDuration, mistDamage, bloodTrailPrefab));
        }
    }
    protected override void UpdateProperties()
    {
        if (currentTier  <= maxTier)
        {
            var tier = tierVariables[currentTier - 1];
            dashSpeed = tier.varFloat;
            dashDuration = tier.duration;
            mistDamage = tier.damage;
            cooldown = tier.cooldown;

        }
    }
}