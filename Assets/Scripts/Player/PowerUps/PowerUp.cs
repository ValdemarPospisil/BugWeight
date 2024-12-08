using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct PowerUpTier
{
    public float damage;
    public float duration; // This will be used as the duration of the slash effect
    public float speed; // This will be used as the radius of the slash effect
}

public abstract class PowerUp : ScriptableObject
{
    public Sprite icon; // Icon for the UI
    public string baseName; // Base name of the power-up
    [TextArea(3, 10)]
    public string baseDescription; // Base description of the power-up
    public int maxTier = 4; // Maximum tier for the power-up (3 upgrade tiers)
    public int currentTier = 1; // Current tier of the power-up (1 is the base tier)

    protected PowerUpManager manager;
    public bool basePicked;

    public List<PowerUpTier> tiers; // List of tier properties

    public void Initialize(PowerUpManager powerUpManager)
    {
        manager = powerUpManager;
    }

    public abstract void Activate(GameObject player);
    public abstract void Deactivate(GameObject player);

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