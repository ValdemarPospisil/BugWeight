using UnityEngine;

public interface IKnockable
{
    void Knockback(Vector2 direction, float force, float duration);
}
