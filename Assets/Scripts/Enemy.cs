using System.Data.Common;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;  // Shared type data

     private Transform player;  // Reference to the player's transform
     
     private GameObject visualChild; // Reference to the instantiated prefab
    private Rigidbody2D rb;     // Reference to the Rigidbody2D component

    public void Initialize(EnemyType type, Vector3 spawnPosition)
    {
        enemyType = type;
        transform.position = spawnPosition;

        // Find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (visualChild == null)
        {
            // Instantiate the prefab as a child of this GameObject and store the reference
            visualChild = Instantiate(type.data.prefab, transform);
        }

        // Access Rigidbody2D and other components from the child
        rb = visualChild.GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError($"Rigidbody2D component not found on prefab '{type.data.prefab.name}'. Make sure it is attached.");
        }
    }

    void Update()
    {
        // Move towards the player or implement other behaviors
    }

    void FixedUpdate()
    {
        

        if (player != null && rb != null)
    {

        // Calculate the direction vector from the enemy's actual position to the player
        Vector2 direction = (player.position - visualChild.transform.position).normalized;

        // Move the enemy towards the player using linearVelocity
        rb.linearVelocity = direction * enemyType.data.moveSpeed;

        // Rotate the enemy to face the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;  // Subtract 90 to align the "head" (assuming it starts pointing up)
    }
    }
}
