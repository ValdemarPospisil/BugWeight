using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Blood Bolt")]
public class BloodBoltPowerUp : PowerUp
{
    public string projectileName; // The projectile to spawn

    public override void Activate(GameObject player)
    {
        UpdateProperties();
        PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
        if (playerShooting != null)
        {
            playerShooting.StartShooting(this);
        }
    }

    public override void Deactivate()
    {
//        PlayerShooting playerShooting = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>();
     //   if (playerShooting != null)
     //   {
      //      playerShooting.StopShooting();
     //   }
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tierVariables.Count)
        {
            var tier = tierVariables[currentTier - 1];
            // No need to store shootInterval and speed separately
        }
    }
}