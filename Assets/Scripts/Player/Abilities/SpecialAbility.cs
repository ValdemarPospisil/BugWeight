using UnityEngine;

public abstract class SpecialAbility : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    [TextArea(3, 10)]
    public string abilityDescription;
    public float cooldown = 5f;
    [SerializeField] protected float percentageIncrease = 0.1f;
    [SerializeField] protected float damage = 5f;
    [SerializeField] protected float duration = 5f;
    public int maxTier = 4;
    public int currentTier = 1; 
    public abstract void Activate();
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