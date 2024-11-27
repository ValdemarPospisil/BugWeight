using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewClassData", menuName = "Player/ClassData")]
public class ClassData : ScriptableObject
{
    public float maxHP;
    public float attackSpeed;
    public float moveSpeed;
    public float attackDamage;

    [System.Serializable]
    public struct ElementalBonus
    {
        public string element;
        public float multiplier;
    }

    public List<ElementalBonus> elementalBonuses;
}
