using UnityEngine;
using System;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private ProjectileTypeData data;
    private Action onDeactivate;
    private Vector3 direction;
    private float lifetime;

    public void Initialize(ProjectileTypeData projectileType)
    {
        data = projectileType;
    }

    public void Launch(Vector3 direction, Action onDeactivateCallback)
    {
        onDeactivate = onDeactivateCallback;
        StartCoroutine(Move(direction));
        this.direction = direction;
        lifetime = data.lifetime;
        RotateToFaceDirection();
    }

    private IEnumerator Move(Vector3 direction)
    {
        while (gameObject.activeSelf)
        {
            transform.position += direction * data.speed * Time.deltaTime;

            // Example collision detection or lifetime check
            if (CheckCollision())
            {
                onDeactivate?.Invoke();
            }

            yield return null;
        }
    }

    private void FixedUpdate () 
    {
        lifetime -= Time.fixedDeltaTime;
        if (lifetime <= 0)
        {
            onDeactivate?.Invoke();
        }
    }

    private void RotateToFaceDirection()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private bool CheckCollision()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, data.speed * Time.fixedDeltaTime);
        if (hit.collider != null && hit.collider.CompareTag(data.targetTag))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(data.damage);
                return true;
            }
        }
        return false;
    }
}
