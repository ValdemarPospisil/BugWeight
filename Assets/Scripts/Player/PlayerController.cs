using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

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
    private bool isWolfForm;
    private float wolfCooldowns;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI cooldownSpaceText;
    [SerializeField] private TextMeshProUGUI cooldownEText;
    [SerializeField] private TextMeshProUGUI cooldownQText;
    [SerializeField] private Image cooldownSpaceCover;
    [SerializeField] private Image cooldownECover;
    [SerializeField] private Image cooldownQCover;
    [SerializeField] private Image abilitySpaceIcon;
    [SerializeField] private Image abilityEIcon;
    [SerializeField] private Image abilityQIcon;
    [SerializeField] private Sprite wolfAbilitySpaceIcon;
    [SerializeField] private Sprite wolfAbilityEIcon;
    [SerializeField] private Sprite wolfAbilityQIcon;
    [SerializeField] private Sprite defaultAbilityIcon;

    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();
    private Dictionary<string, TextMeshProUGUI> cooldownTexts = new Dictionary<string, TextMeshProUGUI>();
    private Dictionary<string, Image> cooldownCovers = new Dictionary<string, Image>();

    
    private bool isBats = false;

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

        cooldownTexts["Space"] = cooldownSpaceText;
        cooldownTexts["E"] = cooldownEText;
        cooldownTexts["Q"] = cooldownQText;

        cooldownCovers["Space"] = cooldownSpaceCover;
        cooldownCovers["E"] = cooldownECover;
        cooldownCovers["Q"] = cooldownQCover;

      
    }

    private void Start()
    {
        trailRenderer.emitting = false;
        isWolfForm = false;
        SetAbilityIcon();
    }

    private void Update()
    {
        UpdateCooldowns();
        HandleInput();
    }

    private void UpdateCooldowns()
    {
        List<string> keys = new List<string>(cooldowns.Keys);
        int index = 0;
        

        foreach (string key in keys)
        {
            if (cooldowns[key] > 0)
            {
                cooldowns[key] -= Time.deltaTime;
                
                cooldownTexts[key].text = Mathf.Ceil(cooldowns[key]).ToString();
                cooldownCovers[key].fillAmount = cooldowns[key] / (isWolfForm ? wolfCooldowns : specialManager.activeAbilities[index].cooldown);
                index++;
            }
            else
            {
                cooldownTexts[key].text = "";
                cooldownCovers[key].fillAmount = 0;
            }
        }

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

        animator.SetFloat("MovementX", lastDirection.x);
        animator.SetFloat("MovementY", lastDirection.y);
        isMoving = movementInput != Vector2.zero;
        if (isWolfForm)
        {
            animator.SetBool("IsMoving", isMoving);
        }
        else
        {
            animator.SetBool("IsMoving", !isMoving);
        }
        if (!isDashing)
        {
            rb.linearVelocity = movementInput * moveSpeed;
        }
        else
        {
            rb.linearVelocity = lastDirection * dashSpeed;
        }

        if (Input.GetKeyDown(KeyCode.E) && (!cooldowns.ContainsKey("Space") || cooldowns["Space"] <= 0))
        {  
            if (isWolfForm)
            {
                animator.SetTrigger("Attack");
                cooldowns["Space"] = wolfCooldowns / 10;
            }
            else if (!cooldowns.ContainsKey("E") || cooldowns["Space"] <= 0)
            {
                if (specialManager.activeAbilities.Count > 0)
                {
                    specialManager.UseSpecialAbility(0);
                    cooldowns["Space"] = specialManager.activeAbilities[0].cooldown; 
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) && (!cooldowns.ContainsKey("E") || cooldowns["E"] <= 0))
        {
            if (isWolfForm)
            {
                animator.SetTrigger("AttackB");
                cooldowns["E"] = wolfCooldowns / 4;
            }
            else if (!cooldowns.ContainsKey("E") || cooldowns["E"] <= 0)
            {
                if (specialManager.activeAbilities.Count > 1)
                {
                    specialManager.UseSpecialAbility(1);
                    cooldowns["E"] = specialManager.activeAbilities[1].cooldown; 
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && (!cooldowns.ContainsKey("Q") || cooldowns["Q"] <= 0))
        {
            if (isWolfForm)
            {
                animator.SetTrigger("Ability");
                cooldowns["Q"] = wolfCooldowns / 2;
            }
            else if (!cooldowns.ContainsKey("Q") || cooldowns["Q"] <= 0)
            {
                if (specialManager.activeAbilities.Count > 2)
                {
                    specialManager.UseSpecialAbility(2);
                    cooldowns["Q"] = specialManager.activeAbilities[2].cooldown; 
                }
            }
        }
    }

    public void SetAbilityIcon()
    {
        if (!isWolfForm)
        {
            if (specialManager.activeAbilities.Count > 0)
            {
                abilitySpaceIcon.sprite = specialManager.activeAbilities[0].icon;
            }
            else
            {
                abilitySpaceIcon.sprite = defaultAbilityIcon;
            }
            if (specialManager.activeAbilities.Count > 1)
            {
                abilityEIcon.sprite = specialManager.activeAbilities[1].icon;
            }
            else
            {
                abilityEIcon.sprite = defaultAbilityIcon;
            }
            if (specialManager.activeAbilities.Count > 2)
            {
                abilityQIcon.sprite = specialManager.activeAbilities[2].icon;
            }
            else
            {
                abilityQIcon.sprite = defaultAbilityIcon;
            }
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
        isBats = true;
        yield return new WaitForSeconds(batsDuration);
        SpeedBoost(-batSpeed);
        if (!isDashing && !isWolfForm)
        {
            spriteRenderer.enabled = true;
        }
        isBats = false;
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
        if (!isBats && !isWolfForm)
        {
             spriteRenderer.enabled = true;
        }
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
        GameObject clone = Instantiate(clonePrefab, transform.position, transform.rotation);
        BloodClone bloodClone = clone.GetComponent<BloodClone>();
        bloodClone.Initialize(cloneDuration, explosionDamage, explosionRadius);
        clone.tag = "Clone";

        spriteRenderer.color = new Color(1, 1, 1, 0.3f);

        targetTag = "Clone";

        yield return new WaitForSeconds(cloneDuration);

        targetTag = "Player";

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public IEnumerator ShadowStep(float teleportDistance, float shadowExplosionDelay, float explosionDamage,
        float explosionRadius, GameObject shadowPrefab, GameObject shadowExplosionEffect)
    {
        Vector2 teleportDirection = lastDirection * teleportDistance;
        Vector2 teleportPosition = (Vector2)transform.position + teleportDirection;

        GameObject shadow = Instantiate(shadowPrefab, transform.position, Quaternion.identity);

        transform.position = teleportPosition;

        yield return new WaitForSeconds(shadowExplosionDelay);

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

    private void ResetCooldowns()
    {
        if (cooldowns.ContainsKey("Space")) cooldowns["Space"] = 0;
        if (cooldowns.ContainsKey("E")) cooldowns["E"] = 0;
        if (cooldowns.ContainsKey("Q")) cooldowns["Q"] = 0;
    }

    private void ChangeToWolfIcons()
    {
        abilitySpaceIcon.sprite = wolfAbilitySpaceIcon;
        abilityEIcon.sprite = wolfAbilityEIcon;
        abilityQIcon.sprite = wolfAbilityQIcon;
    }

    

    public IEnumerator TransformToWolf(float hpBoost, float wolfDuration, GameObject wolfPrefab, float wolfDamage, float freezeRadius, float freezeDuration)
    {
        var playerManager = GetComponent<PlayerManager>();
        playerManager.BuffHealth(hpBoost);
        wolfCooldowns = wolfDuration;
        
        spriteRenderer.enabled = false;
        GameObject wolfInstance = Instantiate(wolfPrefab, transform.position, Quaternion.identity);
        wolfInstance.transform.position = new Vector3(wolfInstance.transform.position.x, wolfInstance.transform.position.y, 0);
        wolfInstance.transform.SetParent(transform);
        Animator wolfAnimator = wolfInstance.GetComponent<Animator>();
        animator = wolfAnimator;
        wolfInstance.GetComponent<WolfForm>().Initialize(wolfDamage, freezeRadius, freezeDuration);
        isWolfForm = true;

        ChangeToWolfIcons();

        Invoke("ResetCooldowns", 0.01f);
        

        yield return new WaitForSeconds(wolfDuration);

        Destroy(wolfInstance);
        spriteRenderer.enabled = true;
        isWolfForm = false;

        playerManager.BuffHealth(-hpBoost);
        animator = GetComponent<Animator>();
        
        ResetCooldowns();
        SetAbilityIcon();
    }

}