using System.Collections;
using UnityEngine;


public class Health : MonoBehaviour, IDamageable
{
    private float health;

    public void SetHealth(float value)
    {
        health = value;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
          Die();
          Debug.Log("The clone is dead");
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        Destroy(gameObject);
    }
}
