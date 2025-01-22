using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyTypeData> enemyTypeDataList;
    [SerializeField] private float spawnRadiusMin = 10f;
    [SerializeField] private float spawnRadiusMax = 20f;
    [SerializeField] private int minDuration = 5;
    [SerializeField] private int maxDuration = 10;
    [SerializeField] private int minEnemies = 1;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private int enemyLimit = 40;

    private Dictionary<EnemyTypeData, List<Enemy>> enemyPools = new Dictionary<EnemyTypeData, List<Enemy>>();

    private void Start()
    {
        InitializeEnemyPools();
        StartCoroutine(StartSpawningEnemies());
    }

    private void InitializeEnemyPools()
    {
        foreach (var data in enemyTypeDataList)
        {
            enemyPools[data] = new List<Enemy>();

            var container = new GameObject(data.typeName + " Container");

            for (int i = 0; i < enemyLimit; i++)
            {
                var enemy = Instantiate(data.prefab).GetComponent<Enemy>();
                enemy.Initialize(data, Vector3.zero, this);
                enemy.gameObject.SetActive(false);
                enemy.transform.SetParent(container.transform);
                enemyPools[data].Add(enemy);
            }
        }
    }

    private IEnumerator StartSpawningEnemies()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            SpawnEnemies();
            yield return new WaitForSeconds(Random.Range(minDuration, maxDuration));
        }
    }

    private void SpawnEnemies()
    {
       
        foreach (var pool in enemyPools)
        {
            var enemyData = pool.Key;
            var enemies = pool.Value;

            int spawnCount = Random.Range(minEnemies, maxEnemies);

            for (int i = 0; i < spawnCount; i++)
            {
                var enemyToSpawn = GetInactiveEnemy(enemies);
                if (enemyToSpawn != null)
                {
                    Vector3 spawnPoint = GetRandomSpawnPosition();
                    enemyToSpawn.Initialize(enemyData, spawnPoint, this);
                    enemyToSpawn.gameObject.SetActive(true);
                }
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(spawnRadiusMin, spawnRadiusMax);
        return transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
    }

    private Enemy GetInactiveEnemy(List<Enemy> pool)
    {
        foreach (var enemy in pool)
        {
            if (!enemy.gameObject.activeSelf)
                return enemy;
        }
        return null;
    }

    public void ReturnEnemy(Enemy enemy, EnemyTypeData enemyTypeData)
    {
        
        if (enemyPools.ContainsKey(enemyTypeData))
        {
            enemyPools[enemyTypeData].Add(enemy);
            enemy.gameObject.SetActive(false);
        }
    }
}
