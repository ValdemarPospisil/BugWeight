using UnityEngine;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour
{
    private Dictionary<string, EnemyType> enemyTypes = new Dictionary<string, EnemyType>();

    public EnemyType GetEnemyType(EnemyTypeData data)
    {
        if (data == null)
        {
            Debug.LogError("EnemyTypeData is null!");
            return null;
        }

        if (!enemyTypes.ContainsKey(data.typeName))
        {
            EnemyType newType = new EnemyType(data);
            enemyTypes[data.typeName] = newType;
        }
        return enemyTypes[data.typeName];
    }

}