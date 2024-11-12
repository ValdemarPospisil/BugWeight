using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public ProjectileFactory projectileFactory;
    public List<ProjectileTypeData> projectileTypeDataList; // List of all available projectile types
    public Transform firePoint;     // Where projectiles will spawn
    public float fireRate = 1f;     // How often to shoot in seconds

    private float fireCooldown;

    private void Start()
    {
        fireCooldown = 0f;
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            ShootProjectiles();
            fireCooldown = fireRate;
        }
    }

    private void ShootProjectiles()
    {
        // Choose a random projectile type
        ProjectileTypeData randomData = projectileTypeDataList[Random.Range(0, projectileTypeDataList.Count)];
        ProjectileType projectileType = projectileFactory.GetProjectileType(randomData);

        // Define 8 directions for shooting (4 orthogonal + 4 diagonal)
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(1, 1).normalized, new Vector2(1, -1).normalized,
            new Vector2(-1, 1).normalized, new Vector2(-1, -1).normalized
        };

        // Spawn a projectile in each direction
        foreach (Vector2 direction in directions)
        {
            // Instantiate a new GameObject for each projectile
            GameObject projectileObject = new GameObject("Projectile");
            projectileObject.transform.position = firePoint.position;
            Projectile projectile = projectileObject.AddComponent<Projectile>();


            projectile.Initialize(projectileType, direction);

            // Attach the visual prefab as a child of the projectile GameObject
           // Instantiate(projectileType.data.prefab, projectileObject.transform);
        }
    }
}
