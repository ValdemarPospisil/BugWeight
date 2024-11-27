using System.Collections;
using UnityEngine;

public class Hunter : PlayerClass
{
    [Header("Hunter-Specific Settings")]
    [SerializeField] private float dashSpeed = 50f;  // Speed multiplier during dash
    [SerializeField] private float dashDuration = 0.2f; // Dash lasts for this time
    [SerializeField] private float dashCooldown = 1f;  // Cooldown before next dash
    [SerializeField] private GameObject arrowPrefab;   // Prefab for the arrow
    [SerializeField] private float arrowDelay = 0.2f;   // Delay before arrow is shot after pressing the button
    [SerializeField] private GameObject afterimagePrefab; // Prefab for the afterimage
    [SerializeField] private ProjectileFactory projectileFactory;
    [SerializeField] private ProjectileTypeData projectileTypeData;
    private PlayerController playerController;

    private Vector2 direction;
    [SerializeField]private Rigidbody2D rb;
    private bool canDash = true;
    private bool canShoot = true;

    protected override void Awake()
{
    base.Awake();
    playerController = GetComponentInParent<PlayerController>();

    

    if (projectileFactory == null || projectileTypeData == null)
    {
        Debug.LogError("Projectile Factory or Type Data not assigned!");
    }
}

    private void Update()
    {
    }

    public override void Special()
    {
        if(canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public override void Attack()
    {
        if (canShoot)
        {
            StartCoroutine(RangedAttack());
        }
    }

    private IEnumerator Dash()
    {
        
        canDash = false;

        direction = playerController.movementInput != Vector2.zero
            ? playerController.movementInput
            : playerController.lastDirection;

        if (direction == Vector2.zero)
        {
            Debug.LogWarning("Cannot dash without a valid direction!");
            canDash = true;
            yield break;
        }


        playerController.enabled = false;

        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            rb.linearVelocity = direction * dashSpeed;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero; 

        playerController.enabled = true;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    private void CreateAfterimage()
    {
        if (afterimagePrefab != null)
        {
            GameObject afterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);
            SpriteRenderer afterimageRenderer = afterimage.GetComponent<SpriteRenderer>();
            SpriteRenderer hunterRenderer = GetComponentInChildren<SpriteRenderer>();

            if (afterimageRenderer != null && hunterRenderer != null)
            {
                afterimageRenderer.sprite = hunterRenderer.sprite;
                afterimageRenderer.color = new Color(1, 1, 1, 0.5f); // Semi-transparent
            }

            Destroy(afterimage, 0.5f); // Destroy after a short time
        }
    }

    private IEnumerator RangedAttack()
    {
        canShoot = false;

        // Simulate a delay to match the animation timing
        yield return new WaitForSeconds(arrowDelay);
        
        if(playerController.isMoving == false)
        {
            ShootArrow();
        }

        // Allow shooting again after a brief cooldown (adjust if needed)
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

   private void ShootArrow()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController reference is missing!");
            return;
        }


        // Use the last direction if no movement input
        direction = playerController.movementInput != Vector2.zero
            ? playerController.movementInput
            : playerController.lastDirection;

        // Create the projectile GameObject
        GameObject projectileObject = new GameObject("Projectile");
        projectileObject.transform.position = transform.position;

        // Fetch the projectile type using the flyweight factory
        if (projectileFactory == null || projectileTypeData == null)
        {
            Debug.LogError("Projectile Factory or Type Data not assigned!");
            return;
        }

        ProjectileType projectileType = projectileFactory.GetProjectileType(projectileTypeData);

        // Add the Projectile script and initialize it
        Projectile projectile = projectileObject.AddComponent<Projectile>();
        projectile.Initialize(projectileType, direction);

    }

    public override void StopAttack()
    {
        // No need to stop the attack for the Hunter
    }
}
