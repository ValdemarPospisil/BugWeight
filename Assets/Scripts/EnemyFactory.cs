using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private Dictionary<string, EnemyType> enemyTypes = new Dictionary<string, EnemyType>();

    public EnemyType GetEnemyType(EnemyTypeData data)
    {
        if (!enemyTypes.ContainsKey(name))
        {
            // Create a new type if it doesn't exist
             EnemyType newType = new EnemyType(data);
            enemyTypes[data.typeName] = newType;
        }
        return enemyTypes[data.typeName];
    }
}
