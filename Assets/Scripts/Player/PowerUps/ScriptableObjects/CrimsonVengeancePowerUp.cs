using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Crimson Vengeance")]
public class CrimsonVengeancePowerUp : PowerUp
{
    public override void Activate(GameObject player)
    {
        UpdateProperties();
        Enemy.explodeOnDeath = true;
    }
    

    public override void Deactivate()
    {
        PlayerManager.explodeOnDeath = false;
        Enemy.explodeOnDeath = false;
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tierVariables.Count)
        {
            var tier = tierVariables[currentTier - 1];
        }

        Enemy.explosionChance = tierVariables[currentTier - 1].variable;
        Enemy.explosionDamage = tierVariables[currentTier - 1].damage;
        

         if (currentTier == maxTier)
        {
            PlayerManager.Instance.AddExtraLives(1, 0.05f); // Add extra life on tier IV
        }
    }
}