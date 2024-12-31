using UnityEngine;

[CreateAssetMenu(fileName = "NewSimpleRangedEnemyData", menuName = "Enemy/Simple Ranged Type Data")]
public class SimpleRangedEnemyData : ScriptableObject
{
    public float projectileSpeed;
    public float attackRange;
    public GameObject projectilePrefab; // Directly store the prefab
    public float baseAttackDamage;
    public float baseMaxHP;
    public float baseXpDrop;
    public float moveSpeed;
    public float attackSpeed;
    public GameObject enemyPrefab;

    public float GetScaledAttackDamage(int level)
    {
        return baseAttackDamage * (1 + (level * 0.015f)); // Slightly less scaling for ranged
    }

    public float GetScaledMaxHP(int level)
    {
        return baseMaxHP * (1 + (level * 0.1f));
    }

    public float GetScaledXpDrop(int level)
    {
        return baseXpDrop * (1 + (level * 0.05f));
    }
}