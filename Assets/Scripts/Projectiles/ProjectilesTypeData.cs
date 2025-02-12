using UnityEngine;

[CreateAssetMenu(fileName = "New ProjectileTypeData", menuName = "Projectile/ProjectileTypeData")]
public class ProjectileTypeData : ScriptableObject
{
    public string typeName;
    public GameObject prefab;
    public float speed;
    public float damage;
    public float lifetime;
    public string targetTag;
    public bool isScalable;
}