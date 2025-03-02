using UnityEngine;
using System;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private ProjectileTypeData data;
    private Action onDeactivate;
    private Vector3 direction;
    private float lifetime = 5f;
    private BoxCollider2D boxCollider;
    private PowerUp powerUp;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D component is missing on the projectile.");
        }
    }

    public void Initialize(ProjectileTypeData projectileType)
    {
        data = projectileType;
        if (data.isScalable)
        {
            PowerUpManager powerUpManager = ServiceLocator.GetService<PowerUpManager>();
            powerUp = powerUpManager.allPowerUps[0];
        }
    }

    public void Launch(Vector3 direction, Action onDeactivateCallback)
    {
        onDeactivate = onDeactivateCallback;
        this.direction = direction.normalized; // Ensure direction is normalized
        lifetime = data.lifetime;
        RotateToFaceDirection();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (gameObject.activeSelf)
        {
            float speed = data.isScalable ? powerUp.tierVariables[powerUp.currentTier - 1].variable : data.speed;
            transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        lifetime -= Time.fixedDeltaTime;
        if (lifetime <= 0)
        {
            onDeactivate?.Invoke();
        }
    }

    private void RotateToFaceDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(data.targetTag))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (data.isScalable)
                {
                    damageable.TakeDamage(powerUp.tierVariables[powerUp.currentTier - 1].damage);
                }
                else
                {
                    damageable.TakeDamage(data.damage);
                }
                onDeactivate?.Invoke();
            }
        }
    }
}