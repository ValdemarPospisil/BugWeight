using Unity.VisualScripting;
using UnityEngine;

public class CrimsonAuraDamage : MonoBehaviour
{
    private PlayerManager playerManager;
    private Animator animator;
    private PowerUp powerUp;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
        if (playerManager == null)
        {
            Debug.LogError("PlayerManager not found in parent.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        animator.SetFloat("tier", powerUp.currentTier);
        if (playerManager != null)
        {
            float healthPercentage = playerManager.GetHealthPercentage();
            float damage = powerUp.tiers[powerUp.currentTier-1].damage * (1 + (1 - healthPercentage));

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, powerUp.tiers[powerUp.currentTier-1].speed);
            foreach (var hitCollider in hitColliders)
            {
                var damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable != null && hitCollider.gameObject.tag == "Enemy")
                {
                    damageable.TakeDamage(damage * Time.deltaTime);
                }
            }
        }
    }

    public void Initialize(PowerUp powerUp)
    {
        this.powerUp = powerUp;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1.4f);
    }
}