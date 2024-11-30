using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "SpecialAbilities/Blood Clone")]
public class BloodCloneAbility : SpecialAbility
{
    [SerializeField] private GameObject bloodClonePrefab;
    [SerializeField] private float cloneDuration = 10f;
    [SerializeField] private float explosionDamage = 50f;
    [SerializeField] private float explosionRadius = 5f;

    public override void Activate()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.StartCoroutine(playerController.UseCloneAbility(bloodClonePrefab, cloneDuration, explosionDamage, explosionRadius));
            }
        }
    }
}