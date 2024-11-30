using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Healing Power-Up")]
public class HealingPowerUp : PowerUp
{
    public float healAmount = 50f;

    public override void Activate(GameObject player)
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        
        Debug.Log("Healing player");
        if (playerManager != null)
        {
            playerManager.Heal(healAmount);
        }
    }

    public override void Deactivate(GameObject player)
    {
        // No deactivation logic needed for healing
    }
}
