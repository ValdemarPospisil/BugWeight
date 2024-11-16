using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerClass
{
    private float blockChance = 0.2f; // 20% chance to block attacks

    private void Start()
    {
        maxHP = 150f;
        attackSpeed = 1.2f;
        moveSpeed = 3f;

        
        elementalBonuses.Add("Earth", 1.2f);  // 20% bonus to earth attacks
        elementalBonuses.Add("Fire", 1.1f);   // 10% bonus to fire attacks
    }

    public override void Attack()
    {
        //animator.SetTrigger("Melee");
        // Implement melee attack logic
        Debug.Log("Warrior attacks with melee weapon.");
    }

    public bool TryBlock()
    {
        return Random.value < blockChance;
    }
}
