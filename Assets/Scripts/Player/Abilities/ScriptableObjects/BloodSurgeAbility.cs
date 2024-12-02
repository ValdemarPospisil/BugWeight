using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood Surge", menuName = "SpecialAbilities/Blood Surge")]
public class BloodSurgeAbility : SpecialAbility
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    //[SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float damageDuration = 3f;
    [SerializeField] private GameObject bloodTrailPrefab;

    public override void Activate()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (player != null)
        {
            playerController.StartCoroutine(playerController.BloodSurge(dashSpeed, dashDuration, damageDuration, bloodTrailPrefab));
        }
    }
}