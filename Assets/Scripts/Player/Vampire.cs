using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : PlayerClass
{
    [Header("Vampire-Specific Settings")]
    [SerializeField] private float damageInterval = 1f; // Time between damage ticks
    [SerializeField] private float damageAmount = 5f;   // Damage per tick
    [SerializeField] private CircleCollider2D damageRadius; // Area of effect
    [SerializeField] private Animator bloodPoolAnimator;    // Pool of blood visual effect

    [SerializeField] private List<Enemy> enemiesInRange = new List<Enemy>(); // Enemies within the radius
   
    protected override void Awake()
    {
        base.Awake();

        if (damageRadius == null)
        {
            Debug.LogError("CircleCollider2D for damageRadius is not assigned!");
        }

        StartCoroutine(DamageNearbyEnemies());
    }


     public override void Special()
    {
        throw new System.NotImplementedException();
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if an enemy enters the radius
        Enemy enemy = collider.GetComponentInParent<Enemy>();
        if (enemy != null && !enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Remove enemy from list when leaving the radius
        Enemy enemy = collider.GetComponentInParent<Enemy>();
        if (enemy != null && enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private IEnumerator DamageNearbyEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);

            // Create a temporary list to hold valid enemies
            List<Enemy> validEnemies = new List<Enemy>();

            // Deal damage to all valid enemies and populate the temporary list
            foreach (var enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                    validEnemies.Add(enemy); // Add only valid (non-null) enemies
                }
            }

            // Replace the original list with the cleaned-up list
            enemiesInRange = validEnemies;

            // Trigger the blood pool animation
            if (bloodPoolAnimator != null)
            {
                bloodPoolAnimator.SetTrigger("Pulse");
            }
        }
    }

    public override void Attack()
    {
        // Vampire doesn't perform direct attacks; handled by passive damage
        Debug.Log("Vampire's attack is passive damage over time.");
    }
    public override void StopAttack()
    {
    }
}
