using UnityEngine;

public class ProjectileChildCollider : MonoBehaviour
{
    private Projectile parentProjectile;

    private void Start()
    {
        // Reference to the parent projectile script
        parentProjectile = GetComponentInParent<Projectile>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (parentProjectile != null)
        {
            // Call a collision handler on the parent to handle damage and destruction
            parentProjectile.HandleCollision(collision.gameObject);
        }
        else
        {
            Debug.Log("parent projectile is null");
        }
    }
}
