using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileType projectileType;  // Shared projectile type data
    private Vector2 direction;             // Direction of movement
    private Rigidbody2D rb;                // Rigidbody for physics-based movement
    private float lifetime = 50f;                // Lifetime counter
     private GameObject visualChild;

    public void Initialize(ProjectileType type, Vector2 direction)
    {
        projectileType = type;
        this.direction = direction;

        if (visualChild == null)
        {
            // Instantiate the prefab as a child of this GameObject and store the reference
            visualChild = Instantiate(type.data.prefab, transform);
        }

        // Access Rigidbody2D and other components from the child
        rb = visualChild.GetComponent<Rigidbody2D>();


        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on the projectile prefab.");
        }
        RotateToFaceDirection();
       // lifetime = projectileType.data.lifetime;
    }

     private void RotateToFaceDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }


    private void FixedUpdate()
    {
        // Move the projectile in the set direction
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileType.data.speed;
        }
        
        // Reduce lifetime and destroy projectile if expired
        lifetime -= Time.fixedDeltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    /*
    private void HandleEnemyCollision(Collider2D other)
    {
        // Handle collision logic, such as damaging an enemy
        Enemy enemy = other.GetComponentInChildren<Enemy>();
        if (enemy != null)
        {
          //  enemy.TakeDamage(projectileType.data.damage);
            Destroy(gameObject);  // Destroy projectile on impact
            Debug.Log("Enemy is not null");
        }
        else
        {
            Debug.Log("enemy is null");
        }
    }
    */
    public void HandleCollision(GameObject other)
{
    Enemy enemy = other.GetComponentInParent<Enemy>();
    if (enemy != null)
    {
       // enemy.DamageEnemy(projectileType.data.damage);
        Destroy(gameObject);  // Destroy projectile on impact
    }
}
}
