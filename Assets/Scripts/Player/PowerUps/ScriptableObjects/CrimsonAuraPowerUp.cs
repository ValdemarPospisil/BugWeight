using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/Crimson Aura")]
public class CrimsonAuraPowerUp : PowerUp
{
    public GameObject auraPrefab; // The aura effect prefab

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.StartCoroutine(ActivateCrimsonAura(playerManager));
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
            // No need to store shootInterval and speed separately
        }
    }

    private IEnumerator ActivateCrimsonAura(PlayerManager playerManager)
    {
        var tier = tiers[currentTier - 1];
        float baseDamage = tier.damage;
        float auraRadius = tier.speed;
        float duration = tier.duration;

        GameObject aura = Instantiate(auraPrefab, playerManager.transform.position, Quaternion.identity);
        aura.transform.SetParent(playerManager.transform);

        while (true)
        {
            float healthPercentage = playerManager.GetHealthPercentage();
            float damage = baseDamage * (1 + (1 - healthPercentage));

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerManager.transform.position, auraRadius);
            foreach (var hitCollider in hitColliders)
            {
                var damageable = hitCollider.GetComponentInParent<IDamageable>();
                if (damageable != null && hitCollider.gameObject.tag == "Enemy")
                {
                    damageable.TakeDamage(damage * Time.deltaTime);
                }
            }

            yield return null;
        }
    }
}