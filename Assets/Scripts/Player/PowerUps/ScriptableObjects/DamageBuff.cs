using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Damage Buff")]
public class DamageBuff : PowerUp
{
    public float damageMultiplier = 1.5f; // How much to multiply the player's damage
    public float duration = 10f; // Duration of the buff

    public override void Activate(GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
           // playerManager.damageMultiplier *= damageMultiplier;
            playerManager.StartCoroutine(RemoveBuffAfterDuration(playerManager));
        }
    }

    public override void Deactivate(GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
          //  playerManager.damageMultiplier /= damageMultiplier;
        }
    }

    private System.Collections.IEnumerator RemoveBuffAfterDuration(PlayerManager playerManager)
    {
        yield return new WaitForSeconds(duration);
        Deactivate(playerManager.gameObject);
    }
}
