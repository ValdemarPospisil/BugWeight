using System.Collections;
using UnityEngine;

public class BloodClone : MonoBehaviour
{
    private float duration;
    private float explosionDamage;
    private float explosionRadius;
    private ParticleSystem explosionEffect;

    public void Initialize(float duration, float explosionDamage, float explosionRadius)
    {
        this.duration = duration;
        this.explosionDamage = explosionDamage;
        this.explosionRadius = explosionRadius;
        explosionEffect = GetComponentInChildren<ParticleSystem>();
        explosionEffect.Stop(); // Stop the particle system so it doesn't play on start
        StartCoroutine(Expire());
    }

    private IEnumerator Expire()
    {
        yield return new WaitForSeconds(duration);
        Explode();
    }

    private void OnDestroy()
    {
        //Explode();
    }

    private void Explode()
    {
        // Play the particle system
        if (explosionEffect != null)
        {
            explosionEffect.transform.parent = null; // Detach the particle system from the clone
            explosionEffect.Play();
            Destroy(explosionEffect.gameObject, explosionEffect.main.duration); // Destroy the particle system after it finishes
        }

        // Damage nearby enemies
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null && hitCollider.gameObject.tag == "Enemy")
            {
                damageable.TakeDamage(explosionDamage);
            }
        }

        // Destroy the clone
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 3.5f);
    }
}