using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    private EnemyCollisionHandler enemyCollisionHandler;
    private Transform playerTransform;
    private Player player;
    private GameObject visualChild;
    private Rigidbody2D rb;
    private float attackCooldown;
    private int enemyCurentHP;

    private void Start() 
    {
        attackCooldown = 0f;
        enemyCollisionHandler = GetComponentInChildren<EnemyCollisionHandler>();
        enemyCurentHP = enemyType.data.enemyMaxHP;
        UpdateHealthUI();
    }

    public void Initialize(EnemyType type, Vector3 spawnPosition)
    {
        enemyType = type;
        transform.position = spawnPosition;
        enemyCurentHP = type.data.enemyMaxHP;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

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

    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null && rb != null)
        {
            Vector2 direction = (playerTransform.position - visualChild.transform.position).normalized;
            rb.linearVelocity = direction * enemyType.data.moveSpeed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90f;
        }
    }

    public void DamageEnemy(int amount) 
    {
        enemyCurentHP -= amount;
        UpdateHealthUI();

        if (enemyCurentHP <= 0)
        {
            player.currentXP += enemyType.data.xpDrop;
            player.UpdateLevelUI();
            Destroy(gameObject);
        } 
    }

    private void UpdateHealthUI()
    {
        if (enemyCollisionHandler.enemyHealthBar != null)
        {
            enemyCollisionHandler.enemyHealthBar.fillAmount = (float)enemyCurentHP / enemyType.data.enemyMaxHP;
        }

        if (enemyCollisionHandler.enemyHealthText != null)
        {
            enemyCollisionHandler.enemyHealthText.text = Mathf.CeilToInt(enemyCurentHP).ToString();
        }
    }

    public void HandlePlayerCollision(GameObject playerObject)
    {
        if (attackCooldown <= 0)
        {
            Player player = playerObject.GetComponent<Player>();
            if (player != null)
            {
                player.DamagePlayer(enemyType.data.attackDamage);
                attackCooldown = 1f / enemyType.data.attackSpeed;
            }
        }
    }
}
