using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileTypeData", menuName = "Projectile/ProjectileTypeData")]
public class ProjectileTypeData : ScriptableObject
{
    public string typeName;
    public GameObject prefab;
    public float speed;            // Speed of the projectile
    public float damage;           // Damage dealt by this projectile
    public float lifetime;         // How long the projectile will live before being destroyed
    public string targetTag;       // Tag of the object that this projectile can damage
}