using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/SpeedBost Power-Up")]
public class SpeedBoostPowerUp : PowerUp
{
    public float speedMultiplier = 1.5f; // How much to multiply the player's damage
    public float duration = 10f; // Duration of the buff

    public override void Activate(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SpeedBoost(speedMultiplier);
           // playerController.StartCoroutine(RemoveBuffAfterDuration(playerController));
        }
    }

    public override void Deactivate(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SpeedBoost(-speedMultiplier);
        }
    }

    private System.Collections.IEnumerator RemoveBuffAfterDuration(PlayerManager playerManager)
    {
        yield return new WaitForSeconds(duration);
        Deactivate(playerManager.gameObject);
    }
}
