using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   public EnemyFactory enemyFactory;
    public List<EnemyTypeData> enemyTypeDataList; // List of ScriptableObject data assets


    public float spawnRadiusMin = 10f;
    public float spawnRadiusMax = 20f;
    public int minEnemies = 1;
    public int maxEnemies = 5;

    public void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        int enemyCount = Random.Range(minEnemies, maxEnemies);

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyTypeData randomData = enemyTypeDataList[Random.Range(0, enemyTypeDataList.Count)];
            EnemyType enemyType = enemyFactory.GetEnemyType(randomData);

            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject enemyObject = new GameObject("Enemy_" + i);
            Enemy enemy = enemyObject.AddComponent<Enemy>();
            enemy.Initialize(enemyType, spawnPosition);
        }
    }
    private Vector3 GetRandomSpawnPosition()
    {
        // Random angle and distance for spawn
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(spawnRadiusMin, spawnRadiusMax);

        // Calculate position around the player
        return transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
    }
}
