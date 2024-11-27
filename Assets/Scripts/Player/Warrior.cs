using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerClass
{
    [Header("Warrior-Specific Settings")]
    [SerializeField] private float meleeRange = 1.5f;
    [SerializeField] private int meleeDamage = 20;
    [SerializeField] private float meleeKnockbackForce = 5f;
    [SerializeField] private LayerMask enemyLayer;     

    [Header("Rage Settings")]
    [SerializeField] private float rageDuration = 5f;
    [SerializeField] private float rageCooldown = 10f;
    [SerializeField] private float rageAttackSpeedMultiplier = 1.5f;
    [SerializeField] private float rageMoveSpeedMultiplier = 2.5f;
    [SerializeField] private float rageAttackDamageMultiplier = 1.4f;
    [SerializeField] private int rageExtraArmor = 10;

    private bool canRage = true;

    private float originalAttackSpeed;
    private float originalMoveSpeed;
    private float originalAttackDamage;
    private SpriteRenderer spriteRage;

    private void Start()
    {
        originalAttackSpeed = attackSpeed;
        originalMoveSpeed = moveSpeed;
        originalAttackDamage = attackDamage;
        spriteRage = GetComponent<SpriteRenderer>();
        spriteRage.enabled = false;
    }

    public override void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, meleeRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit {enemy.name}!");

            IDamageable damageable = enemy.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(meleeDamage);
            }
            else
            {
                Debug.Log("Damagable is null");
            }

            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDirection * meleeKnockbackForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("EnemyRB is null");
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
    public override void Special()
    {
        if (canRage)
        {
            StartCoroutine(Rage());
        }
        else
        {
            Debug.Log("Rage is on cooldown!");
        }
    }

    private IEnumerator Rage()
    {
        Debug.Log("Warrior enters Rage mode!");
        canRage = false;

        attackSpeed *= rageAttackSpeedMultiplier;
        moveSpeed *= rageMoveSpeedMultiplier;
        attackDamage *= rageAttackDamageMultiplier;
        AddArmor(rageExtraArmor);
        
        spriteRage.enabled = true;

        yield return new WaitForSeconds(rageDuration);

        attackSpeed = originalAttackSpeed;
        moveSpeed = originalMoveSpeed;
        attackDamage = originalAttackDamage;
        RemoveArmor(rageExtraArmor);

        Debug.Log("Rage mode ends.");

        yield return new WaitForSeconds(rageCooldown);
        canRage = true;
        spriteRage.enabled = false;
        Debug.Log("Rage is ready to use again!");
    }

    private void AddArmor(int amount)
    {
        Debug.Log($"Armor increased by {amount}. Implement armor effect if necessary.");
    }

    private void RemoveArmor(int amount)
    {
        Debug.Log($"Armor decreased by {amount}. Revert any armor-related effects.");
    }

    public override void StopAttack()
    {
        
    }
}

