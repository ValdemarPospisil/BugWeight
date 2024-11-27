using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Beam : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 0.5f; // Damage every 0.5 seconds
    [SerializeField] private LayerMask enemyLayer;
    private BoxCollider2D boxCollider;
    private List<IDamageable> enemiesInRange = new List<IDamageable>();
    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D is not attached to the Beam GameObject.");
        }

        StartCoroutine(DamageEnemies());
    }

    private void OnEnable() {
        StartCoroutine(DamageEnemies());
    } 

    private void Update()
    {
        // Update the beam's rotation based on the player's movement input
        Vector2 direction = playerController.movementInput;

        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IDamageable damageable = collider.GetComponentInParent<IDamageable>();
        if (damageable != null && !enemiesInRange.Contains(damageable))
        {
            enemiesInRange.Add(damageable);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        IDamageable damageable = collider.GetComponentInParent<IDamageable>();
        if (damageable != null && enemiesInRange.Contains(damageable))
        {
            enemiesInRange.Remove(damageable);
        }
    }

    private IEnumerator DamageEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);

            // Create a temporary list to hold valid enemies
            List<IDamageable> validEnemies = new List<IDamageable>();

            // Deal damage to all valid enemies and populate the temporary list
            foreach (var enemy in enemiesInRange)
            {
                if (enemy != null && ((MonoBehaviour)enemy) != null && ((MonoBehaviour)enemy).gameObject.activeInHierarchy)
                {
                    enemy.TakeDamage(damage);
                    validEnemies.Add(enemy); // Add only valid (non-null) enemies
                }
                else
                {
                    Debug.Log("Enemy is null or inactive");
                }
            }

            // Replace the original list with the cleaned-up list
            enemiesInRange = validEnemies;
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}