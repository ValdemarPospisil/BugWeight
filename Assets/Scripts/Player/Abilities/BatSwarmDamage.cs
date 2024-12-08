using UnityEngine;

public class BatSwarmDamage : MonoBehaviour
{
    private float damage;
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IDamageable>().TakeDamage(damage);
        }
    }
}
