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
    private float dashSpeed;
    private BatSwarmDamage batSwarmDamageScript;
    public Vector2 lastDirection { get; private set; } = new Vector2(1, 0);
    private string targetTag = "Player";

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
        if (batSwarmDamageScript != null) batSwarmDamageScript.gameObject.SetActive(false);
    }

    private void Start()
    {
        trailRenderer.emitting = false;
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        NotifyEnemies(targetTag);
    }

    public void SpeedBoost(float multiplier)
    {
        moveSpeed += multiplier;
    }

    private Vector2 GetMovementInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void HandleInput()
    {
        Vector2 movementInput = GetMovementInput();
        if (movementInput != Vector2.zero)
        {
            lastDirection = movementInput;
        }

        animator.SetFloat("MovementX", movementInput.x);
        animator.SetFloat("MovementY", movementInput.y);
        isMoving = movementInput != Vector2.zero;
        animator.SetBool("IsMoving", !isMoving);
        if (!isDashing)
        {
            rb.linearVelocity = movementInput * moveSpeed;
        }
        else
        {
            rb.linearVelocity = lastDirection * dashSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            specialManager.UseSpecialAbility(0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            specialManager.UseSpecialAbility(1);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            specialManager.UseSpecialAbility(2);
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

    public IEnumerator BloodSurge(float surgeSpeed, float dashDuration, float damageDuration, GameObject bloodTrailPrefab)
    {
        if (isDashing) yield break;
        dashSpeed = surgeSpeed;

        capsuleCollider2D.isTrigger = true;

        isDashing = true;
        var bloodTrail = Instantiate(bloodTrailPrefab, transform.position, Quaternion.identity);
        bloodTrail.transform.SetParent(transform);
        spriteRenderer.enabled = false;

        var trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashDuration);
        spriteRenderer.enabled = true;
        rb.linearVelocity = Vector2.zero;
        bloodTrail.GetComponent<ParticleSystem>().Stop();
        trailRenderer.emitting = false;
        bloodTrail.transform.SetParent(null);
        capsuleCollider2D.isTrigger = false;
        isDashing = false;

        Destroy(bloodTrail, damageDuration);
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

        targetTag = "Clone";

        // Wait for the clone duration
        yield return new WaitForSeconds(cloneDuration);

        targetTag = "Player";

        spriteRenderer.color = new Color(1, 1, 1, 1);
        //Destroy(clone);
    }

    public IEnumerator ShadowStep(float teleportDistance, float shadowExplosionDelay, float explosionDamage,
        float explosionRadius, GameObject shadowPrefab, GameObject shadowExplosionEffect)
    {
        Vector2 teleportDirection = lastDirection * teleportDistance;
        Vector2 teleportPosition = (Vector2)transform.position + teleportDirection;

        // Instantiate the shadow at the player's current position
        GameObject shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);

        // Teleport the player
        transform.position = teleportPosition;

        // Wait for the shadow explosion delay
        yield return new WaitForSeconds(shadowExplosionDelay);

        // Explode the shadow
        ExplodeShadow(shadow, explosionDamage, explosionRadius, shadowExplosionEffect);
    }

    private void ExplodeShadow(GameObject shadow, float explosionDamage, float explosionRadius, GameObject shadowExplosionEffect)
    {
        GameObject explosion = Instantiate(shadowExplosionEffect, shadow.transform.position, Quaternion.identity);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(explosion.transform.position, explosionRadius);
        Animator shadowAnimator = shadow.GetComponent<Animator>();
        ParticleSystem shadowParticle = shadow.GetComponent<ParticleSystem>();
        shadowParticle.Stop();
        shadowAnimator.SetTrigger("Fade");
        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponentInParent<IDamageable>();
            if (damageable != null && hitCollider.gameObject.tag == "Enemy")
            {
                damageable.TakeDamage(explosionDamage);
            }
        }

        // Destroy the shadow object
        Destroy(shadow, 0.4f);
        Destroy(explosion, 0.5f);
    }

    private void NotifyEnemies(string targetTag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SetTargetTag(targetTag);
            }
        }
    }
}