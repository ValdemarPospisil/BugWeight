using UnityEngine;

[CreateAssetMenu(fileName = "NewMeleeEnemyData", menuName = "Enemy/Melee Type Data")]
public class MeleeEnemyData : EnemyTypeData
{
    public float meleeAttackRange;
    public float knockbackStrength;

    // Example override if melee needs unique scaling
    public override float GetScaledAttackDamage(int level)
    {
        return baseAttackDamage * (1 + (level * 0.02f)); // Higher scaling for melee
    }
}
