using UnityEngine;

public abstract class SpecialAbility : ScriptableObject
{
    public string abilityName;
    public Sprite icon;
    [TextArea(3, 10)]
    public string abilityDescription;
    public abstract void Activate();
}
