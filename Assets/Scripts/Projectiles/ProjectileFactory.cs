using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    [SerializeField] private List<ProjectileTypeData> projectileTypes;
    [SerializeField] private int initialPoolSize = 10;

    private Dictionary<string, Queue<Projectile>> projectilePools = new Dictionary<string, Queue<Projectile>>();

    private void Start()
    {
        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var type in projectileTypes)
        {
            var pool = new Queue<Projectile>();
            var container = new GameObject(type.typeName + " Container");

            for (int i = 0; i < initialPoolSize; i++) // Initial pool size
            {
                var projectile = Instantiate(type.prefab).GetComponent<Projectile>();
                projectile.gameObject.SetActive(false);
                projectile.Initialize(type);
                pool.Enqueue(projectile);
                projectile.transform.SetParent(container.transform);
            }

            projectilePools[type.typeName] = pool;
        }
    }

    public Projectile SpawnProjectile(string typeName, Vector3 position, Vector2 direction)
    {
        if (!projectilePools.ContainsKey(typeName))
        {
            Debug.LogError("Projectile type not found: " + typeName);
            return null;
        }

        var pool = projectilePools[typeName];
        Projectile projectile;

        if (pool.Count > 0)
        {
            projectile = pool.Dequeue();
        }
        else
        {
            var type = projectileTypes.Find(t => t.name == typeName);
            if (type == null)
            {
                Debug.LogError("Projectile type not found: " + typeName);
                return null;
            }

            projectile = Instantiate(type.prefab).GetComponent<Projectile>();
            projectile.Initialize(type);
        }

        projectile.transform.position = position;
        projectile.gameObject.SetActive(true);
        projectile.Launch(direction, () => ReturnProjectile(typeName, projectile));
        return projectile;
    }

    public void ReturnProjectile(string typeName, Projectile projectile)
    {
        projectile.gameObject.SetActive(false);

        if (projectilePools.ContainsKey(typeName))
        {
            projectilePools[typeName].Enqueue(projectile);
        }
    }
}