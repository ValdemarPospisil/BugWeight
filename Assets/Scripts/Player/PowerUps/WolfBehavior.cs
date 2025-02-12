using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(CircleCollider2D))]
public class WolfBehavior : MonoBehaviour
{
    [SerializeField] private float patrolRange = 5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float moveSpeed = 3f;

    private GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private GameObject currentTarget;
    private Vector3 patrolTarget;
    private float lastAttackTime;
    private CircleCollider2D detectionRange;

    public void Initialize(GameObject player, float damage, float patrolRange, float speed)
    {
        this.player = player;
        attackDamage = damage;
        this.patrolRange = patrolRange;
        moveSpeed = speed;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        detectionRange = GetComponent<CircleCollider2D>();

        detectionRange.isTrigger = true;
        detectionRange.radius = patrolRange;

        SetNewPatrolTarget();
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            MoveTowards(currentTarget.transform.position);
            if (IsWithinAttackRange(currentTarget))
            {
                Attack(currentTarget);
            }
        }
        else
        {
            Patrol();
            FindClosestEnemy();
        }

        UpdateAnimation();
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            SetNewPatrolTarget();
        }

        MoveTowards(patrolTarget);
    }

    private void SetNewPatrolTarget()
    {
        Vector3 randomDirection = Random.insideUnitCircle * patrolRange;
        if (player != null)
        {
            patrolTarget = player.transform.position + randomDirection;
        }
        patrolTarget.z = 0; // Ensure the wolf stays in the same plane
    }

    private void MoveTowards(Vector3 target)
    {
        Vector2 direction = (target - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void Attack(GameObject enemy)
    {
        StopMovement();

        if (Time.time > lastAttackTime + attackCooldown)
        {
            // Trigger attack animation
            animator.SetTrigger("Attack");

            enemy.GetComponent<IDamageable>()?.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }

    private void FindClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange.radius)
            .Where(c => c.CompareTag("Enemy"))
            .ToArray();

        if (hits.Length > 0)
        {
            currentTarget = hits.OrderBy(h => Vector2.Distance(h.transform.position, transform.position))
                                .FirstOrDefault()?.gameObject;
        }
    }

    private bool IsWithinAttackRange(GameObject target)
    {
        return Vector3.Distance(transform.position, target.transform.position) < 1f;
    }

    private void UpdateAnimation()
    {
        Vector2 velocity = rb.linearVelocity;

        if (velocity.magnitude > 0.1f)
        {
            animator.SetFloat("MovementX", velocity.x);
            animator.SetFloat("MovementY", velocity.y);
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentTarget)
        {
            currentTarget = null;
        }
    }
}
