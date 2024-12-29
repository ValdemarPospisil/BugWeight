using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable, IFreezable, IKnockable
{
    private EnemyType enemyType;
    private Transform targetTransform;
    private Rigidbody2D rb;
    private float attackCooldown;
    private float enemyCurrentHP;
    private float enemyMaxHP;
    private float attackDamage;
    private float xpDrop;
    private string targetTag = "Player";
    private bool enemyCanMove = true;
    private bool isKnockedBack = false;
    private LevelManager levelManager;
    private bool isDead = false;
    public static event System.Action<Vector3> OnEnemyKilled;

    // Merged from EnemyCollisionHandler.cs
    [SerializeField] private Image enemyHealthBar;
    private Canvas enemyCanvas;
    [SerializeField] private GameObject deathParticles;

    private MeleeEnemyData meleeData;
    private RangedEnemyData rangedData;

    private void Start()
    {
        attackCooldown = 0f;
        
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        enemyCanvas = GetComponentInChildren<Canvas>();
        enemyCanMove = true;
        rb = GetComponent<Rigidbody2D>();

        UpdateHealthUI();
        enemyCanvas.enabled = false;
        FindTarget();
    }

    private void InitializeEnemyData()
    {
        if(enemyType == null) Debug.LogError("Enemy Type is null");

        if (enemyType.data is MeleeEnemyData)
        {
            meleeData = (MeleeEnemyData)enemyType.data;
        }
        else if (enemyType.data is RangedEnemyData)
        {
            rangedData = (RangedEnemyData)enemyType.data;
        }
    }

    public void Initialize(EnemyType type, Vector3 spawnPosition)
    {
        levelManager = ServiceLocator.GetService<LevelManager>();
        enemyType = type;
        transform.position = spawnPosition;

        attackDamage = type.data.GetScaledAttackDamage(levelManager.level);
        enemyMaxHP = type.data.GetScaledMaxHP(levelManager.level);
        xpDrop = type.data.GetScaledXpDrop(levelManager.level);
        enemyCurrentHP = enemyMaxHP;


        InitializeEnemyData();
        
    }

    private void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isKnockedBack) return;
        if (!enemyCanMove || targetTransform == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (targetTransform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;


        if (enemyType.data.behaviorType == EnemyBehaviorType.Melee)
        {
            rb.linearVelocity = direction * enemyType.data.moveSpeed;
        }
        else if (enemyType.data.behaviorType == EnemyBehaviorType.Ranged)
        {
            if (Vector2.Distance(transform.position, targetTransform.position) <= rangedData.attackRange)
            {
                rb.linearVelocity = Vector2.zero;
                if (attackCooldown <= 0)
                {
                    Attack();
                    attackCooldown = 1f / enemyType.data.attackSpeed;
                }
            }
            else
            {
                rb.linearVelocity = direction * enemyType.data.moveSpeed;
            }
        }
    }

    private IEnumerator ApplyKnockback(Vector2 direction, float knockbackForce, float knockbackDuration)
    {
        if (rb == null) yield break;

        isKnockedBack = true;

        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
        isKnockedBack = false;
    }

   

    public void SetTargetTag(string newTargetTag)
    {
        targetTag = newTargetTag;
        FindTarget();
    }

    private void FindTarget()
    {
        TargetingSystem targetingSystem = ServiceLocator.GetService<TargetingSystem>();
        targetTransform = targetingSystem.FindTarget(targetTag);
    }

    public void TakeDamage(float amount)
    {
        if (this == null) return;
        enemyCurrentHP -= amount;
        UpdateHealthUI();

        if (enemyCurrentHP <= 0)
        {
            if (!isDead)
            {
                isDead = true;
                Die();
            }
        }
    }

    public void Freeze(float duration)
    {
        if (this == null) return;
        StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        enemyCanMove = false;
        yield return new WaitForSeconds(duration);
        enemyCanMove = true;
    }

    public void Knockback(Vector2 direction, float knockbackForce, float knockbackDuration)
    {
        if (this == null) return;
        StartCoroutine(ApplyKnockback(direction, knockbackForce, knockbackDuration));
    }

    private void Die()
    {
        DisableCollision();
        OnEnemyKilled?.Invoke(transform.position);

        Instantiate(deathParticles, transform.position, Quaternion.identity);
        levelManager.AddXP(xpDrop);

        Destroy(gameObject);
    }

    private void UpdateHealthUI()
    {
        enemyCanvas.enabled = true;
        if (enemyHealthBar != null)
        {
            enemyHealthBar.fillAmount = enemyCurrentHP / enemyMaxHP;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && this != null && enemyType.data.behaviorType == EnemyBehaviorType.Melee)
        {
            if (attackCooldown <= 0)
            {
                IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                    attackCooldown = 1f / enemyType.data.attackSpeed;
                }
            }
        }
    }

    public void DisableCollision()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void Attack()
    {
        if (meleeData != null)
        {
            if (Vector2.Distance(transform.position, targetTransform.position) <= meleeData.meleeAttackRange)
            {
                // Perform melee attack logic
                IDamageable target = targetTransform.GetComponent<IDamageable>();
                target?.TakeDamage(meleeData.GetScaledAttackDamage(levelManager.level));

                // Apply knockback (if needed)
                if (target is IKnockable knockable)
                {
                    Vector2 direction = (targetTransform.position - transform.position).normalized;
                    knockable.Knockback(direction, meleeData.knockbackStrength, 0.5f);
                }
            }
        }
        else if (rangedData != null)
        {
            if (Vector2.Distance(transform.position, targetTransform.position) <= rangedData.attackRange)
            {
                // Perform ranged attack logic
                GameObject projectile = Instantiate(rangedData.projectilePrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                Vector2 direction = (targetTransform.position - transform.position).normalized;

                rb.linearVelocity = direction * rangedData.projectileSpeed;

                // Assign damage to projectile
                Projectile proj = projectile.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.Initialize(direction, rangedData.projectileSpeed, rangedData.GetScaledAttackDamage(levelManager.level));
                }
            }
        }
    }
}