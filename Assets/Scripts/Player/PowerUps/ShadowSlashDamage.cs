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
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.0f, 1.0f), 0f); // Adjust size as needed
        foreach (var hitCollider in hitColliders)
        {
            var damageable = hitCollider.GetComponentInParent<IDamageable>();
            if (damageable != null && hitCollider.gameObject.tag == "Enemy")
            {
                damageable.TakeDamage(damage);
            }
        }
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