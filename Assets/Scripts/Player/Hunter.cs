using System.Collections.Generic;
using UnityEngine;

public class Hunter : PlayerClass
{
    private float dodgeChance = 0.3f; // 30% chance to dodge attacks

    private void Start()
    {
        maxHP = 100f;
        attackSpeed = 1.5f;
        moveSpeed = 8f;
        
        elementalBonuses.Add("Wind", 1.3f);   // 30% bonus to wind attacks
        elementalBonuses.Add("Wood", 1.1f);   // 10% bonus to wood attacks
    }

    public override void Attack()
    {
        animator.SetTrigger("Ranged");
        // Implement ranged attack logic
        Debug.Log("Hunter attacks with ranged weapon.");
    }

    public bool TryDodge()
    {
        return Random.value < dodgeChance;
    }
}
