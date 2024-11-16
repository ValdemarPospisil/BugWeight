using System.Collections.Generic;
using UnityEngine;

public class Vampire : PlayerClass
{
    private float damageAuraInterval = 1.0f; // Damage aura triggers every second
    private float auraDamage = 10f;

    private void Start()
    {
        maxHP = 120f;
        attackSpeed = 0.8f;
        moveSpeed = 3f;
        
//        elementalBonuses.Add("Blood", 1.3f); // 30% bonus to blood attacks
  //      elementalBonuses.Add("Dark", 1.2f);  // 20% bonus to dark attacks
        
        InvokeRepeating(nameof(DamageAura), damageAuraInterval, damageAuraInterval);
    }

    public override void Attack()
    {
        // Vampire doesn’t have a traditional attack, only an aura
        Debug.Log("Vampire’s aura deals damage over time.");
    }

    private void DamageAura()
    {
        // Implement area damage logic around the Vampire
        Debug.Log("Vampire deals aura damage to nearby enemies.");
    }
}
