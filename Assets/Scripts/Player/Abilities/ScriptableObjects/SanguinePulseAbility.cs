using UnityEngine;

[CreateAssetMenu(fileName = "Sanguine Pulse", menuName = "SpecialAbilities/SanguinePulse")]
public class SanguinePulseAbility : SpecialAbility
{
    private float damage = 10f;
    private float pullForce = 5f;
    private float pulseRadius = 5f; 
    [SerializeField] private GameObject abilityPrefab;

    public override void Activate()
    {
        UpdateProperties();
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found.");
            return;
        }
        // Instantiate the ability prefab at the player's position
        GameObject instantiatedPrefab = Instantiate(abilityPrefab, player.transform.position, Quaternion.identity);
        instantiatedPrefab.GetComponent<ParticleSystem>().Play();

        // Find enemies within the pulse radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(player.transform.position, pulseRadius);
        int enemiesHit = 0;

        foreach (var hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Vector2 pushDirection = (enemy.transform.position - player.transform.position).normalized;
                
                enemy.Knockback(-pushDirection, pullForce, 0.5f);

                enemy.TakeDamage(damage);
                
                enemiesHit++;
            }
            
        }
        Destroy(instantiatedPrefab, 0.5f);

    }

    protected override void UpdateProperties()
    {
        if (currentTier < maxTier)
        {
            var tier = tierVariables[currentTier - 1];
            damage = tier.damage;
            pullForce = tier.duration;
            pulseRadius = tier.varFloat;
            cooldown = tier.cooldown;
        }
    }
}