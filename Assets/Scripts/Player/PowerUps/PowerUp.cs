using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct PowerUpTier
{
    public float damage;
    public float duration;
    public float speed; 
}

public enum GroupType
{
    Blood,
    Shadow,
    Wolf,
    Necromatic
}

public abstract class PowerUp : ScriptableObject
{
    public GroupType group;
    public Sprite icon;
    public string baseName;
    [TextArea(3, 10)]
    public string baseDescription;
    public int maxTier = 4;
    public int currentTier = 1; 

    protected PowerUpManager manager;
    public bool basePicked;

    public List<PowerUpTier> tiers;

    public void Initialize(PowerUpManager powerUpManager)
    {
        manager = powerUpManager;
    }

    public abstract void Activate(GameObject player);
    public abstract void Deactivate();

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