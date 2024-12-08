using UnityEngine;

public class BloodExplosion : MonoBehaviour
{
    private float damage;
    private float radius;
    private float duration;

    private void Start()
    {
        // Trigger the explosion effect
        
    }

    public void SetUpExplosion (float damage, float radius, float duration)
    {
        this.damage = damage;
        this.radius = radius;
        this.duration = duration;

        Explode();
        // Destroy the explosion object after the duration
        Destroy(gameObject, duration);
    }
    
    private void Explode()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}