using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable, IFreezable, IKnockable
{
    private EnemyTypeData enemyData;
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
    private CapsuleCollider2D capsuleCollider;
    [SerializeField] private Image enemyHealthBar;
    private Canvas enemyCanvas;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject[] enemyCoins;
    [SerializeField] private int minAmountOfCoins;
    [SerializeField] private int maxAmountOfCoins;

    private MeleeEnemyData meleeData;
    private RangedEnemyData rangedData;
    private ProjectileFactory projectileFactory;
    private EnemySpawner enemySpawner;

    private void OnEnable()
    {
        attackCooldown = 0f;
        projectileFactory = ServiceLocator.GetService<ProjectileFactory>();
        levelManager = ServiceLocator.GetService<LevelManager>();
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        
        enemyCanMove = true;
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        capsuleCollider.enabled = true;

        enemyCanMove = true;
        isKnockedBack = false;

        enemyCanvas = GetComponentInChildren<Canvas>();

        isDead = false;
        gameObject.SetActive(true);
        ResetEnemy();
        FindTarget();
        UpdateHealthUI();

    }

    private void InitializeEnemyData()
    {
        if(enemyData == null) Debug.Log("Enemy Type is null");

        if (enemyData is MeleeEnemyData)
        {
            meleeData = (MeleeEnemyData)enemyData;
        }
        else if (enemyData is RangedEnemyData)
        {
            rangedData = (RangedEnemyData)enemyData;
        }
    }

    public void Initialize(EnemyTypeData data, Vector3 spawnPosition, EnemySpawner spawner)
    {
        enemyData = data;
        enemySpawner = spawner;

        transform.position = spawnPosition;
        ResetEnemy();
        InitializeEnemyData();
    }

    private void ResetEnemy()
    {
        if (enemyData == null) return;

        attackDamage = enemyData.GetScaledAttackDamage(levelManager.level);
        xpDrop = enemyData.GetScaledXpDrop(levelManager.level);
        enemyMaxHP = enemyData.GetScaledMaxHP(levelManager.level);
        enemyCurrentHP = enemyMaxHP;

        gameObject.SetActive(true);
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
        if (enemyData.behaviorType == EnemyBehaviorType.Melee)
        {
            rb.linearVelocity = direction * enemyData.moveSpeed;
        }
        else if (enemyData.behaviorType == EnemyBehaviorType.Ranged)
        {
            if (rangedData == null) Debug.Log("Ranged Data is null");
            if (Vector2.Distance(transform.position, targetTransform.position) <= rangedData.attackRange)
            {
                
                rb.linearVelocity = Vector2.zero;
                if (attackCooldown <= 0)
                {
                    Attack();
                    attackCooldown = 1f / enemyData.attackSpeed;
                }
            }
            else
            {
                rb.linearVelocity = direction * enemyData.moveSpeed;
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

        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        int randomAmountOfCoins = Random.Range(minAmountOfCoins, maxAmountOfCoins);

        for(int i = 0; i < randomAmountOfCoins; i++) {
            int rand = Random.Range(0, enemyCoins.Length);
            Instantiate(enemyCoins[rand], transform.position, Quaternion.identity);
        }

        levelManager.AddXP(xpDrop);
        enemySpawner.ReturnEnemy(this, enemyData);
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
        if (collision.gameObject.CompareTag("Player") && this != null && enemyData.behaviorType == EnemyBehaviorType.Melee)
        {
            if (attackCooldown <= 0)
            {
                IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                    attackCooldown = 1f / enemyData.attackSpeed;
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
        Vector2 direction = (targetTransform.position - transform.position).normalized;

        if (projectileFactory == null) Debug.Log("Projectile Factory is null");
        projectileFactory.SpawnProjectile(
            rangedData.projectilename,
            transform.position,
            direction                       
        );

    }

}