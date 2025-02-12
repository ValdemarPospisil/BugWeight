using UnityEngine;

public class ShadowSlashDamage : MonoBehaviour
{
    private float damage;
    private bool isDoubleDamage;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("double", isDoubleDamage);

    }

    public void DealDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.5f); // Adjust size as needed
        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null && hitCollider.gameObject.tag == "Enemy")
            {
                damageable.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

    public void DestroySlash()
    {
        Destroy(gameObject);
    }

    public void Initialize(float damage, bool isDoubleDamage)
    {
        this.damage = damage;
        this.isDoubleDamage = isDoubleDamage;
    }
}