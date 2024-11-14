using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCollisionHandler : MonoBehaviour
{
    private Enemy enemy;
    public Image enemyHealthBar;
    public TextMeshProUGUI enemyHealthText;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && enemy != null)
        {
            enemy.HandlePlayerCollision(collision.gameObject);
        }
    }
}
