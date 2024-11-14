using UnityEngine;

public class Wizard : PlayerClass
{
    private void Start()
    {
        maxHP = 80f;
        attackSpeed = 1.0f;
        moveSpeed = 3.5f;
        
        elementalBonuses.Add("Thunder", 1.4f); // 40% bonus to thunder attacks
        elementalBonuses.Add("Ice", 1.3f);     // 30% bonus to ice attacks
        elementalBonuses.Add("Dark", 1.2f);    // 20% bonus to dark attacks
    }

    public override void Attack()
    {
        animator.SetTrigger("Magic");
        // Implement magic attack logic with elemental projectiles
        Debug.Log("Wizard casts elemental magic.");
    }
}
