using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ClassData", menuName = "ScriptableObjects/ClassData", order = 1)]
public class ClassData : ScriptableObject
{
    public float maxHP;
    public float attackSpeed;
    public float moveSpeed;
    public Dictionary<string, float> elementalBonuses;  // Optional for elements
}
