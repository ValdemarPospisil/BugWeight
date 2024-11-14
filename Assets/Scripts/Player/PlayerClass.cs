using UnityEngine;
using System.Collections.Generic;
public abstract class PlayerClass : MonoBehaviour
{
    [SerializeField] private ClassData classData;

    protected float maxHP;
    protected float attackSpeed;
    protected float moveSpeed;
    protected Dictionary<string, float> elementalBonuses;
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        if (classData != null)
        {
            maxHP = classData.maxHP;
            attackSpeed = classData.attackSpeed;
            moveSpeed = classData.moveSpeed;
            elementalBonuses = classData.elementalBonuses;
        }
        else
        {
            Debug.LogError("ClassData not assigned for " + this.GetType().Name);
        }
    }

    public abstract void Attack(); // Placeholder for subclasses to implement
}
