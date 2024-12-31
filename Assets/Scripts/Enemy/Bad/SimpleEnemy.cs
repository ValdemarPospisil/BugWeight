using UnityEngine;
using System.Collections;

public class SimpleEnemy : MonoBehaviour, IDamageable, IFreezable, IKnockable
{
    private SimpleRangedEnemyData rangedData;
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

    [SerializeField] private GameObject deathParticles;

    private void Start()
    {
        attackCooldown = 0f;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        enemyCanMove = true;
        rb = GetComponent<Rigidbody2D>();

        FindTarget();
    }

    public void Initialize(SimpleRangedEnemyData data, Vector3 spawnPosition)
    {
        levelManager = ServiceLocator.GetService<LevelManager>();
        rangedData = data;
        transform.position = spawnPosition;

        attackDamage = data.GetScaledAttackDamage(levelManager.level);
        enemyMaxHP = data.GetScaledMaxHP(levelManager.level);
        xpDrop = data.GetScaledXpDrop(levelManager.level);
        enemyCurrentHP = enemyMaxHP;
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

        if (Vector2.Distance(transform.position, targetTransform.position) <= rangedData.attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            if (attackCooldown <= 0)
            {
                Attack();
                attackCooldown = 1f / rangedData.attackSpeed;
            }
        }
        else
        {
            rb.linearVelocity = direction * rangedData.moveSpeed;
        }
    }

    private void Attack()
    {
        if (Vector2.Distance(transform.position, targetTransform.position) <= rangedData.attackRange)
        {
            GameObject projectile = Instantiate(rangedData.projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector2 direction = (targetTransform.position - transform.position).normalized;

            rb.linearVelocity = direction * rangedData.projectileSpeed;

            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.Initialize(direction, rangedData.projectileSpeed, rangedData.GetScaledAttackDamage(levelManager.level), "Player");
            }
        }
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

    private void Die()
    {
        OnEnemyKilled?.Invoke(transform.position);

        Instantiate(deathParticles, transform.position, Quaternion.identity);
        levelManager.AddXP(xpDrop);

        Destroy(gameObject);
    }
}