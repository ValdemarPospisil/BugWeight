using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBost Power-Up")]
public class SpeedBoostPowerUp : PowerUp
{
    public float speedMultiplier = 1.5f; // How much to multiply the player's damage
    public float duration = 10f; // Duration of the buff

    public override void Activate(GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.speedMultiplier *= speedMultiplier;
            playerManager.StartCoroutine(RemoveBuffAfterDuration(playerManager));
        }
    }

    public override void Deactivate(GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.speedMultiplier /= speedMultiplier;
        }
    }

    private System.Collections.IEnumerator RemoveBuffAfterDuration(PlayerManager playerManager)
    {
        yield return new WaitForSeconds(duration);
        Deactivate(playerManager.gameObject);
    }
}
