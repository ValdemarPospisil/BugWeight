using UnityEngine;

[CreateAssetMenu(fileName = "Death Toll", menuName = "SpecialAbilities/Death Toll")]
public class DeathToll : SpecialAbility
{

    private KillCounter killCounter;

    public override void Activate()
    {
        UpdateProperties();
        killCounter = ServiceLocator.GetService<KillCounter>();
        killCounter.DeathToll(cooldown);
        
    }
    protected override void UpdateProperties()
    {
        if (currentTier < maxTier)
        {
            cooldown = tierVariables[currentTier - 1].cooldown;
            
        }
    }
}