using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    private Vector2 direction; // Direction of movement
    private Rigidbody2D rb; // Rigidbody for physics-based movement
    private float speed; // Speed of the projectile
    private float damage; // Damage dealt by this projectile
    private float lifetime = 5f; // Lifetime counter
    private string targetTag; // Tag of the target object

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed, float damage, string targetTag)
    {
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;
        this.targetTag = targetTag;

        RotateToFaceDirection();
    }

    private void RotateToFaceDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, speed * Time.fixedDeltaTime);
        if (hit.collider != null && hit.collider.CompareTag(targetTag))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        // Reduce lifetime and destroy projectile if expired
        lifetime -= Time.fixedDeltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

}