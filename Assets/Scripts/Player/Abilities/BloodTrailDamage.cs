using System.Collections;
using UnityEngine;

public class BloodTrailDamage : MonoBehaviour
{
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float damageDuration = 2f;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                StartCoroutine(ApplyDamageOverTime(damageable));
            }
        }
    }

    public void SetDamage (float damage) 
    {
        damagePerSecond = damage;
    }

    private IEnumerator ApplyDamageOverTime(IDamageable damageable)
    {
        float elapsedTime = 0f;
        while (elapsedTime < damageDuration)
        {
            if (damageable != null)
            {
                damageable.TakeDamage(damagePerSecond * Time.deltaTime);
            }
            else
            {
                // Exit the coroutine if the damageable object is null
                yield break;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}