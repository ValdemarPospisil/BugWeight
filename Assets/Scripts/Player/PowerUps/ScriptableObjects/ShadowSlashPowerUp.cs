using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "PowerUps/Shadow Slash")]
public class ShadowSlashPowerUp : PowerUp
{
    public GameObject slashPrefab; // The slash effect prefab

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.StartCoroutine(PerformShadowSlash(playerController));
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

    private IEnumerator PerformShadowSlash(PlayerController playerController)
    {
        var tier = tiers[currentTier - 1];
        float damage = tier.damage;
        float duration = tier.duration;
        float speed = tier.speed;

        // Perform the shadow slash based on the current tier
        switch (currentTier)
        {
            case 0:
                // Perform single slash in front
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.identity);
                break;
            case 1:
                // Perform double slash in front
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.identity);
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, 180));
                break;
            case 2:
                // Perform double slash in front and back
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.identity);
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, 180));
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, 90));
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, -90));
                break;
            case 3:
                // Perform double slash in all directions
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.identity);
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, 180));
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, 90));
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, -90));
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, 45));
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, -45));
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, 135));
                Instantiate(slashPrefab, playerController.transform.position, Quaternion.Euler(0, 0, -135));
                break;
        }

        yield return new WaitForSeconds(duration);

        // Apply damage to enemies hit by the slashes
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerController.transform.position, speed);
        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponentInParent<IDamageable>();
            if (damageable != null && hitCollider.gameObject.tag == "Enemy")
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}