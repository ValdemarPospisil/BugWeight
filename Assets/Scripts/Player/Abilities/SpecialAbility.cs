using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct TierVariableSA
{
    public float damage;
    public float duration;
    public float varFloat; 
    public float cooldown;
}
public abstract class SpecialAbility : ScriptableObject
{
    public GroupType group;
    public string abilityName;
    public Sprite icon;
    [TextArea(3, 10)]
    public string abilityDescription;
    public List<TierVariableSA> tierVariables;
    public int maxTier = 4;
    public int currentTier = 1; 
    public abstract void Activate();
    public float cooldown { get; protected set; }
    public bool basePicked;

     public bool CanUpgrade()
    {
        return currentTier < maxTier;
    }

    public void Upgrade()
    {
        if (CanUpgrade())
        {
            currentTier++;
            // Update the skill description and other properties based on the new tier
            UpdateProperties();
        }
    }

    protected abstract void UpdateProperties();
}