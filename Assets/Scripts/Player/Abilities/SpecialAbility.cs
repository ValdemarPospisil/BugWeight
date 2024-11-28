using UnityEngine;

public abstract class SpecialAbility : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    public abstract void Activate();
}
