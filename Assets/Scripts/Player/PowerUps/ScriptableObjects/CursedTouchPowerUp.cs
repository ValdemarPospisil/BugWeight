using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Cursed Touch")]
public class CursedTouchPowerUp : PowerUp
{
    private float cursedDamage;
    private float curseChance;
    public override void Activate(GameObject player)
    {
        UpdateProperties();
        
    }
    

    public override void Deactivate()
    {
    }

    protected override void UpdateProperties()
    {
        if (currentTier < tierVariables.Count)
        {
            var tier = tierVariables[currentTier - 1];
            cursedDamage = tier.damage;
            curseChance = tier.variable;
        }
        Enemy.curseChance = curseChance;
        Enemy.cursedDamage = cursedDamage;
        Enemy.isCursedTouchOn = true;

    }
}