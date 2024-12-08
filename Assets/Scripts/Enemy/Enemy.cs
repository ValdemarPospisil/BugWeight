using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyType enemyType;
    private Transform targetTransform;
    private Rigidbody2D rb;
    private float attackCooldown;
    private float enemyCurrentHP;
    private string targetTag = "Player";
    public bool enemyCanMove = true;
    private bool isKnockedBack = false;
    private LevelManager levelManager;
    private bool isDead = false;
    public static event System.Action<Vector3> OnEnemyKilled;

    // Merged from EnemyCollisionHandler.cs
    [SerializeField] private Image enemyHealthBar;
    private Canvas enemyCanvas;
    [SerializeField] private GameObject deathParticles;

    private void Start()
    {
        attackCooldown = 0f;
        
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        enemyCanvas = GetComponentInChildren<Canvas>();

        levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelManager not found in the scene.");
        }

        

        UpdateHealthUI();
        enemyCanvas.enabled = false;
        FindTarget();
    }

    public void Initialize(EnemyType type, Vector3 spawnPosition)
    {
        enemyType = type;
        enemyCurrentHP = enemyType.data.enemyMaxHP;
        transform.position = spawnPosition;
        enemyCurrentHP = type.data.enemyMaxHP;


        rb = GetComponent<Rigidbody2D>();
        
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
        if (targetTransform == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (targetTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * enemyType.data.moveSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

    public IEnumerator ApplyKnockback(Vector2 direction, float knockbackForce, float knockbackDuration)
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

    private void FindTarget()
    {
        GameObject targetObject = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObject != null)
        {
            targetTransform = targetObject.transform;
        }
        else
        {
            Debug.LogError($"{targetTag} not found in the scene.");
        }
    }

    public void SetTargetTag(string newTargetTag)
    {
        targetTag = newTargetTag;
        FindTarget();
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

    private void Die()
    {
        DisableCollision();
        OnEnemyKilled?.Invoke(transform.position);

        Instantiate(deathParticles, transform.position, Quaternion.identity);
        levelManager.AddXP(enemyType.data.xpDrop);

        Destroy(gameObject);
    }

    private void UpdateHealthUI()
    {
        enemyCanvas.enabled = true;
        if (enemyHealthBar != null)
        {
            enemyHealthBar.fillAmount = enemyCurrentHP / enemyType.data.enemyMaxHP;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && this != null)
        {
            if (attackCooldown <= 0)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(enemyType.data.attackDamage);
                attackCooldown = 1f / enemyType.data.attackSpeed;
            }
        }
        }
    }

    public void DisableCollision()
    {
        GetComponent<Collider2D>().enabled = false;
    }
}