using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BloodClone : MonoBehaviour, IDamageable
{
    private float duration;
    private float explosionDamage;
    [SerializeField] private float explosionRadius;
    private float cloneHealth;
    private ParticleSystem explosionEffect;

    public void Initialize(float duration, float explosionDamage, float cloneHealth)
    {
        this.duration = duration;
        this.explosionDamage = explosionDamage;
        this.cloneHealth = cloneHealth;
        explosionEffect = GetComponentInChildren<ParticleSystem>();
        explosionEffect.Stop(); // Stop the particle system so it doesn't play on start
        StartCoroutine(Expire());
    }

    private IEnumerator Expire()
    {
        yield return new WaitForSeconds(duration);
        Explode();
    }


    private void FixedUpdate()
    {
        if (cloneHealth <= 0)
        {
            PlayerController playerController = ServiceLocator.GetService<PlayerController>();
            playerController.ChangeToNormal();
            Explode();
        }
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

    public void TakeDamage(float damage)
    {
        cloneHealth -= damage;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}