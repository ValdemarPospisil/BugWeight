using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyType enemyType;
    private EnemyCollisionHandler enemyCollisionHandler;
    private Transform targetTransform;
    private Rigidbody2D rb;
    private float attackCooldown;
    private float enemyCurrentHP;
    private GameObject visualChild;
    private string targetTag = "Player";
    public bool enemyCanMove = true;
    private bool isKnockedBack = false;
    private LevelManager levelManager;
    private bool isDead = false;
    private ParticleSystem deathParticles;
    private SpriteRenderer sprite;
    private void Start()
    {
        attackCooldown = 0f;
        enemyCollisionHandler = GetComponentInChildren<EnemyCollisionHandler>();
        enemyCurrentHP = enemyType.data.enemyMaxHP;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        deathParticles = GetComponentInChildren<ParticleSystem>();
        deathParticles.Stop();
        sprite = GetComponentInChildren<SpriteRenderer>();

        // Find LevelManager
        levelManager = FindFirstObjectByType<LevelManager>();
        if (levelManager == null)
        {
            Debug.LogError("LevelManager not found in the scene.");
        }

        UpdateHealthUI();
        FindTarget();
    }

    public void Initialize(EnemyType type, Vector3 spawnPosition)
    {
        enemyType = type;
        transform.position = spawnPosition;
        enemyCurrentHP = type.data.enemyMaxHP;

        if (visualChild == null)
        {
            visualChild = Instantiate(type.data.prefab, transform);
        }

        rb = visualChild.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"Rigidbody2D component not found on prefab '{type.data.prefab.name}'. Make sure it is attached.");
        }
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

        Vector2 direction = (targetTransform.position - visualChild.transform.position).normalized;
        
        rb.linearVelocity = direction * enemyType.data.moveSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

  
    public IEnumerator ApplyKnockback(Vector2 direction, float knockbackForce, float knockbackDuration)
    {
        if (rb == null) yield break;

        isKnockedBack = true;

        // Apply force
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

        // Wait for knockback duration
        yield return new WaitForSeconds(knockbackDuration);

        // Stop movement after knockback
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
                StartCoroutine(Die());
            }
        }
    }
    
    private IEnumerator Die()
    {
        enemyCollisionHandler.DisableCollision();
        deathParticles.Play();
        levelManager.AddXP(enemyType.data.xpDrop);
        
        sprite.enabled = false;
        enemyCollisionHandler.enemyHealthBar.transform.parent.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject); 
    }


    private void UpdateHealthUI()
    {
        if (enemyCollisionHandler != null && enemyCollisionHandler.enemyHealthBar != null)
        {
            enemyCollisionHandler.enemyHealthBar.fillAmount = enemyCurrentHP / enemyType.data.enemyMaxHP;
        }
    }

    public void HandlePlayerCollision(GameObject playerObject)
    {
        if (attackCooldown <= 0)
        {
            IDamageable damageable = playerObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(enemyType.data.attackDamage);
                attackCooldown = 1f / enemyType.data.attackSpeed;
            }
        }
    }
}