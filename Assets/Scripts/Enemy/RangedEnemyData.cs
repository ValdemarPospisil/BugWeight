using UnityEngine;

[CreateAssetMenu(fileName = "NewRangedEnemyData", menuName = "Enemy/Ranged Type Data")]
public class RangedEnemyData : EnemyTypeData
{
    public float projectileSpeed;
    public float attackRange;
    public GameObject projectilePrefab;

    // Example override if ranged needs unique scaling
    public override float GetScaledAttackDamage(int level)
    {
        return baseAttackDamage * (1 + (level * 0.015f)); // Slightly less scaling for ranged
    }
}
