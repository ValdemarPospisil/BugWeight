using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private Enemy enemy;

    private void Start()
    {
        // Get reference to the parent Enemy script
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Pass collision to the Enemy script
        if (collision.gameObject.CompareTag("Player") && enemy != null)
        {
            enemy.HandlePlayerCollision(collision.gameObject);
        }
    }
}
