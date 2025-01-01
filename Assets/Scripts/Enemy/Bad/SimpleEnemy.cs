using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimpleEnemy : MonoBehaviour, IDamageable, IFreezable, IKnockable
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackDamage;
    [SerializeField] private float enemyMaxHP;
    [SerializeField] private float xpDrop;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private EnemyBehaviorType enemyClass;
    [SerializeField] private Image enemyHealthBar;
    private Transform targetTransform;
    private Rigidbody2D rb;
    private float attackCooldown;
    private float enemyCurrentHP;
    private string targetTag = "Player";
    private bool enemyCanMove = true;
    private bool isKnockedBack = false;
    private LevelManager levelManager;
    private bool isDead = false;
    public static event System.Action<Vector3> OnEnemyKilled;
    private Canvas enemyCanvas;

    [SerializeField] private GameObject deathParticles;

    private void Start()
    {
        attackCooldown = 0f;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        enemyCanMove = true;
        rb = GetComponent<Rigidbody2D>();
        levelManager = ServiceLocator.GetService<LevelManager>();
        enemyCurrentHP = enemyMaxHP;
        enemyCanvas = GetComponentInChildren<Canvas>();

        FindTarget();
    }

    public void Initialize(Vector3 spawnPosition)
    {
        
        transform.position = spawnPosition;
        
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
        if (enemyClass == EnemyBehaviorType.Melee)
        {
            rb.linearVelocity = direction * moveSpeed;
        }
        else if (enemyClass == EnemyBehaviorType.Ranged)
        {
            if (Vector2.Distance(transform.position, targetTransform.position) <= attackRange)
            {
                
                rb.linearVelocity = Vector2.zero;
                if (attackCooldown <= 0)
                {
                    Attack();
                    attackCooldown = 1f / attackSpeed;
                }
            }
            else
            {
                rb.linearVelocity = direction * moveSpeed;
            }
        }
    }

    private void Attack()
    {
        if (Vector2.Distance(transform.position, targetTransform.position) <= attackRange)
        {
           Vector2 direction = (targetTransform.position - transform.position).normalized;

            GameObject projectileGO = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            SimpleProjectile proj = projectileGO.GetComponent<SimpleProjectile>();

            proj.Initialize(direction, projectileSpeed, projectileDamage, "Player");

            rb.linearVelocity = direction * projectileSpeed;
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
            UpdateHealthUI();
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

     private void UpdateHealthUI()
    {
        if (enemyCanvas == null) Debug.Log("Enemy Canvas is null");
        enemyCanvas.enabled = true;
        if (enemyHealthBar != null)
        {
            enemyHealthBar.fillAmount = enemyCurrentHP / enemyMaxHP;
        }
    }


     private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && this != null && enemyClass == EnemyBehaviorType.Melee)
        {
            if (attackCooldown <= 0)
            {
                IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                    attackCooldown = 1f / attackSpeed;
                }
            }
        }
    }

    public void DisableCollision()
    {
        GetComponent<Collider2D>().enabled = false;
    }

}