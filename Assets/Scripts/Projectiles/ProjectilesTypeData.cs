using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileTypeData", menuName = "Projectile/ProjectileTypeData")]
public class ProjectileTypeData : ScriptableObject
{
    public string typeName;
    public GameObject prefab;      // Prefab for the projectile visuals
    public float speed;            // Speed of the projectile
    public float damage;           // Damage dealt by this projectile
}