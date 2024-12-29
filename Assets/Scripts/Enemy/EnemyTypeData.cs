using UnityEngine;
public enum EnemyBehaviorType
{
    Melee,
    Ranged
}

public class EnemyTypeData : ScriptableObject
{
    public string typeName;
    public GameObject prefab;
    public EnemyBehaviorType behaviorType;

    public float baseAttackDamage;
    public float baseMaxHP;
    public float baseXpDrop;
    public float moveSpeed;
    public float attackSpeed;

    public virtual float GetScaledAttackDamage(int level)
    {
        return baseAttackDamage * (1 + (level * 0.01f));
    }

    public virtual float GetScaledMaxHP(int level)
    {
        return baseMaxHP * (1 + (level * 0.01f));
    }

    public virtual float GetScaledXpDrop(int level)
    {
        return baseXpDrop * (1 + (level * 0.01f));
    }
}
