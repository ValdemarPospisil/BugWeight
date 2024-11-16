using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;

public abstract class PlayerClass : MonoBehaviour
{
    [SerializeField] private ClassData classData;
    public RuntimeAnimatorController  animatorController; // Reference to Animator

    protected float maxHP;
    protected float attackSpeed;
    protected float moveSpeed;
    protected Dictionary<string, float> elementalBonuses;
    //protected Animator animator;

    protected virtual void Awake()
    {
        
        if (classData != null)
        {
            maxHP = classData.maxHP;
            attackSpeed = classData.attackSpeed;
            moveSpeed = classData.moveSpeed;
            elementalBonuses = new Dictionary<string, float>();
            foreach (var bonus in classData.elementalBonuses)
            {
                elementalBonuses[bonus.element] = bonus.multiplier;
            }
        }
        else
        {
            Debug.LogError("ClassData not assigned for " + this.GetType().Name);
        }

        if (animatorController == null)
        {
            Debug.LogError("Animator not assigned for " + this.GetType().Name);
        }

      //  animator = GetComponentInParent<Animator>();
    }

    public abstract void Attack();

    public virtual float GetMoveSpeed(float baseSpeed)
    {
        return moveSpeed > 0 ? moveSpeed : baseSpeed;
    }

   
}
