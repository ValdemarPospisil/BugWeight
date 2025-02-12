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
            playerController.StartCoroutine(SpawnSlashes(playerController));
        }
    }

    private IEnumerator SpawnSlashes(PlayerController playerController)
    {
        while (true)
        {
            // Instantiate slash in front of the player
            InstantiateSlash(playerController, playerController.GetCurrentDirection());

            // If current tier is 4, instantiate slash behind the player too
            if (currentTier == 4)
            {
                InstantiateSlash(playerController, -playerController.GetCurrentDirection());
            }

            yield return new WaitForSeconds(interval);
        }
    }

    private void InstantiateSlash(PlayerController playerController, Vector2 direction)
    {
        GameObject slash = Instantiate(slashPrefab, playerController.transform.position, Quaternion.identity);
        slash.transform.SetParent(playerController.transform);
        slash.transform.localPosition = new Vector2(direction.x * 1.3f, direction.y * 1.3f);
        slash.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);

        var slashDamage = slash.GetComponent<ShadowSlashDamage>();
        if (slashDamage != null)
        {
            slashDamage.Initialize(damage, isDoubleDamage);
        }
    }

    public override void Deactivate()
    {
        // No deactivation needed for this power-up
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tierVariables.Count)
        {
            var tier = tierVariables[currentTier - 1];
            damage = tier.damage;
            interval = tier.variable;
            isDoubleDamage = currentTier >= 2;
        }
    }
}