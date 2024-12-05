using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/Shadow Slash")]
public class ShadowSlashPowerUp : PowerUp
{
    public GameObject slashPrefab; // The slash effect prefab
    private float damage;
    private float interval;
    private bool isDoubleDamage;

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
             // Assuming tier 2 is double damage

            playerController.StartCoroutine(SpawnSlashes(playerController));
        }
    }

    private IEnumerator SpawnSlashes(PlayerController playerController)
    {
        while (true)
        {
            GameObject slash = Instantiate(slashPrefab, playerController.transform.position, Quaternion.identity);
            slash.transform.SetParent(playerController.transform);
            slash.transform.localPosition = new Vector2(playerController.lastDirection.x * 1.3f, playerController.lastDirection.y *  1.3f);
           
            slash.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(playerController.lastDirection.y, playerController.lastDirection.x) * Mathf.Rad2Deg - 90);

            var slashDamage = slash.GetComponent<ShadowSlashDamage>();
            if (slashDamage != null)
            {
                slashDamage.Initialize(damage, isDoubleDamage);
            }

            yield return new WaitForSeconds(interval);
        }
    }

    public override void Deactivate(GameObject player)
    {
        // No deactivation needed for this power-up
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tiers.Count)
        {
            var tier = tiers[currentTier - 1];
            damage = tier.damage;
            interval = tier.duration;
            isDoubleDamage = currentTier >= 2;
        }
    }
}