using UnityEngine;
using System.Linq;
using System.Collections;

public class WolfForm : MonoBehaviour
{
    [SerializeField] private GameObject wolfPrefab; // The wolf prefab to spawn
    private float attackDamage = 10f;
    private float freezeRadius = 5;
    private float freezeDuration = 2;
    [SerializeField] private float summonOffset = 1f;

    
    public void Initialize(float attackDamage, float freezeRadius, float freezeDuration)
    {
        this.attackDamage = attackDamage;
        this.freezeRadius = freezeRadius;
        this.freezeDuration = freezeDuration;
    }

    public void Attack()
    {
        // Implement normal attack logic here
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f)
            .Where(c => c.CompareTag("Enemy"))
            .ToArray();

        foreach (var hit in hits)
        {
            hit.GetComponent<IDamageable>()?.TakeDamage(attackDamage);
        }
    }

    public void AttackB()
    {
        // Implement freeze enemies logic here
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, freezeRadius)
            .Where(c => c.CompareTag("Enemy"))
            .ToArray();

        foreach (var hit in hits)
        {
            hit.GetComponent<IFreezable>()?.Freeze(freezeDuration);
        }
    }

    public void Ability()
    {
        // Implement summon 2 more hounds logic here
        Vector3 spawnPosition1 = transform.position + new Vector3(summonOffset, 0, 0);
        Vector3 spawnPosition2 = transform.position + new Vector3(-summonOffset, 0, 0);

        GameObject wolf1 = Instantiate(wolfPrefab, spawnPosition1, Quaternion.identity);
        GameObject wolf2 = Instantiate(wolfPrefab, spawnPosition2, Quaternion.identity);

        WolfBehavior wolfBehavior1 = wolf1.GetComponent<WolfBehavior>();
        WolfBehavior wolfBehavior2 = wolf2.GetComponent<WolfBehavior>();

        wolfBehavior1.Initialize(gameObject, attackDamage, freezeRadius, summonOffset);
        wolfBehavior2.Initialize(gameObject, attackDamage, freezeRadius, summonOffset);

        StartCoroutine(KillWolves(wolf1, wolf2));
        
    }

    private IEnumerator KillWolves(GameObject wolf1, GameObject wolf2)
    {
        yield return new WaitForSeconds(8f);
        Destroy(wolf1);
        Destroy(wolf2);
        Debug.Log("Wolves have been killed");
    }
}