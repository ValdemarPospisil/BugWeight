using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyType enemyType;
    private EnemyCollisionHandler enemyCollisionHandler;
    private Transform playerTransform;
    private PlayerManager player;
    private GameObject visualChild;
    private Rigidbody2D rb;
    private float attackCooldown;
    private float enemyCurentHP;

    private void Start() 
    {
        attackCooldown = 0f;
        enemyCollisionHandler = GetComponentInChildren<EnemyCollisionHandler>();
       // enemyCurentHP = enemyType.data.enemyMaxHP;
        UpdateHealthUI();

        
    }

    public void Initialize(EnemyType type, Vector3 spawnPosition)
    {
        enemyType = type;
        transform.position = spawnPosition;
        enemyCurentHP = type.data.enemyMaxHP;

        FindPlayer();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        if (visualChild == null)
        {
            visualChild = Instantiate(type.data.prefab, transform);
        }

        rb = visualChild.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"Rigidbody2D component not found on prefab '{type.data.prefab.name}'. Make sure it is attached.");
        }

        Wizard.OnPlayerInvisible += HandlePlayerInvisible;
    }

    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        Wizard.OnPlayerInvisible -= HandlePlayerInvisible;
    }

    void FixedUpdate()
    {
        if (playerTransform == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        Vector2 direction = (playerTransform.position - visualChild.transform.position).normalized;
        rb.linearVelocity = direction * enemyType.data.moveSpeed;
        // Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), visualChild.GetComponent<Collider2D>());

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

    private void FindPlayer()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void HandlePlayerInvisible()
    {
        playerTransform = null; // Clear reference to the 
        FindPlayer();
    }


    public void TakeDamage(float amount) 
    {
        enemyCurentHP -= amount;
        UpdateHealthUI();
        Debug.Log("Enemy took damage");

        if (enemyCurentHP <= 0)
        {
            EnemyDeath();
        } 
    }
    private void EnemyDeath () {
        player.currentXP += enemyType.data.xpDrop;
        player.UpdateUI();
        this.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void UpdateHealthUI()
    {
        enemyCollisionHandler.enemyHealthBar.fillAmount = enemyCurentHP / enemyType.data.enemyMaxHP;
    }

    public void HandlePlayerCollision(GameObject playerObject)
    {
        if (attackCooldown <= 0)
        {
            IDamageable damageable = playerObject.GetComponent<IDamageable>();
            if (player != null)
            {   
                damageable.TakeDamage(enemyType.data.attackDamage);
                attackCooldown = 1f / enemyType.data.attackSpeed;
            }
            else
            {
                Debug.Log("player je null :(");
            }
        }
    }
}
