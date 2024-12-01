using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    private bool isDashing;
    private SpecialAbilityManager specialManager;
    private PowerUpManager powerUpManager;
    private Animator animator;
    private TrailRenderer trailRenderer;
    private bool isMoving;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    private BatSwarmDamage batSwarmDamageScript;

    public bool IsDashing
    {
        get => isDashing;
        set => isDashing = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        specialManager = ServiceLocator.GetService<SpecialAbilityManager>();
        powerUpManager = ServiceLocator.GetService<PowerUpManager>();
        trailRenderer = GetComponent<TrailRenderer>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        batSwarmDamageScript = GetComponentInChildren<BatSwarmDamage>();
        batSwarmDamageScript.gameObject.SetActive(false);
    }

    private void Start()
    {
        trailRenderer.emitting = false;
    }

    private void Update()
    {
        HandleInput();
    }

    public void SpeedBoost(float multiplier)
    {
        moveSpeed += multiplier;
    }

    private void HandleInput()
    {
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        animator.SetFloat("MovementX", movementInput.x);
        animator.SetFloat("MovementY", movementInput.y);
        isMoving = movementInput != Vector2.zero;
        animator.SetBool("IsMoving", !isMoving);
        if (!isDashing)
        {
            rb.linearVelocity = movementInput * moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            specialManager.UseSpecialAbility();
        }
    }

    public void BatSwarm(float damage, float batsDuration, float batSpeed)
    {
        
        batSwarmDamageScript.SetDamage(damage);
        StartCoroutine(TransformToBats(batsDuration, batSpeed));
    }

    private IEnumerator TransformToBats(float batsDuration, float batSpeed)
    {
        batSwarmDamageScript.gameObject.SetActive(true);
        SpeedBoost(batSpeed);
        capsuleCollider2D.enabled = false;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(batsDuration);
        SpeedBoost(-batSpeed);
        spriteRenderer.enabled = true;
        capsuleCollider2D.enabled = true;
        batSwarmDamageScript.gameObject.SetActive(false);
    }

    public IEnumerator UseCloneAbility(GameObject clonePrefab, float cloneDuration, float explosionDamage, float explosionRadius)
    {
        // Instantiate the clone
        GameObject clone = Instantiate(clonePrefab, transform.position, transform.rotation);
        BloodClone bloodClone = clone.GetComponent<BloodClone>();
        bloodClone.Initialize(cloneDuration, explosionDamage, explosionRadius);
        clone.tag = "Clone";

        // Make the player invisible
        spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        gameObject.tag = "Invisible";

        // Notify enemies to target the clone
        NotifyEnemies("Clone");

        // Wait for the clone duration
        yield return new WaitForSeconds(cloneDuration);

        // Destroy the clone
        Destroy(clone);

        // Make the player visible again
        spriteRenderer.color = new Color(1, 1, 1, 1);
        gameObject.tag = "Player";

        // Notify enemies to target the player again
        NotifyEnemies("Player");
    }

    private void NotifyEnemies(string targetTag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponentInParent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SetTargetTag(targetTag);
            }
        }
    }
}