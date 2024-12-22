using UnityEngine;
using System.Collections;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "Sanguine Pulse", menuName = "SpecialAbilities/SanguinePulse")]
public class SanguinePulseAbility : SpecialAbility
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float healAmount = 5f;
    [SerializeField] private float pulseRadius = 5f; 
    [SerializeField] private GameObject abilityPrefab;

    public override void Activate()
    {
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
                // Push the enemy back
                Vector2 pushDirection = (enemy.transform.position - player.transform.position).normalized;
                
                enemy.Knockback(pushDirection, pushForce, duration);

                enemy.TakeDamage(damage);
                
                // Increment the count of enemies hit
                enemiesHit++;
            }
            
        }
        player.GetComponent<MonoBehaviour>().StartCoroutine(enumerator(instantiatedPrefab));


        // Heal the player for each enemy hit
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.Heal(enemiesHit * healAmount);
        }

    }

    private IEnumerator enumerator(GameObject instantiatedPrefab)
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(instantiatedPrefab);
    }

    protected override void UpdateProperties()
    {
        if (currentTier < maxTier)
        {
            pushForce += pushForce * percentageIncrease;
            healAmount += healAmount * percentageIncrease;
            duration -= duration * percentageIncrease;
            cooldown -= cooldown * percentageIncrease;
            damage += damage * percentageIncrease;
        }
    }
}