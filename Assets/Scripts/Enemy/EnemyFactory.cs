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

    public GameObject CreateEnemy(EnemyType enemyType, Vector3 spawnPosition, string targetTag)
    {
        if (enemyType == null || enemyType.data == null)
        {
            Debug.LogError("EnemyType or EnemyTypeData is null during enemy creation!");
            return null;
        }

        GameObject enemyObject = Instantiate(enemyType.data.prefab, spawnPosition, Quaternion.identity);
        Enemy enemy = enemyObject.GetComponent<Enemy>();

        enemy.Initialize(enemyType, spawnPosition);
        enemy.SetTargetTag(targetTag); // Set the target tag for the enemy
        return enemyObject;
    }
}