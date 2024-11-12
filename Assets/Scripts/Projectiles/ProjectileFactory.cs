using System.Collections.Generic;
using UnityEngine;

public class ProjectileFactory : MonoBehaviour
{
    private Dictionary<string, ProjectileType> projectileTypes = new Dictionary<string, ProjectileType>();

    public ProjectileType GetProjectileType(ProjectileTypeData data)
    {
        if (!projectileTypes.ContainsKey(data.typeName))
        {
            // Create a new projectile type if it doesn't already exist
            ProjectileType newType = new ProjectileType(data);
            projectileTypes[data.typeName] = newType;
        }
        return projectileTypes[data.typeName];
    }
}
