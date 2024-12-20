using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyFactory enemyFactory;
    public List<EnemyTypeData> enemyTypeDataList; // List of ScriptableObject data assets

    [SerializeField] private float spawnRadiusMin = 10f;
    [SerializeField] private float spawnRadiusMax = 20f;
    [SerializeField] private int minDuration = 5;
    [SerializeField] private int maxDuration = 10;
    [SerializeField] private int minEnemies = 1;
    [SerializeField] private int maxEnemies = 5;
    private LevelManager levelManager;

    private void Awake() {
        levelManager = ServiceLocator.GetService<LevelManager>();
    }
    private void Start () {
        StartCoroutine(StartSpawningEnemies());
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
        int enemyCount = Random.Range(minEnemies, maxEnemies);
        enemyCount = enemyCount + Random.Range(0, levelManager.level * 2);

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyTypeData randomData = enemyTypeDataList[Random.Range(0, enemyTypeDataList.Count)];
            EnemyType enemyType = enemyFactory.GetEnemyType(randomData);

            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject enemyObject = Instantiate(enemyType.data.prefab, spawnPosition, Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.Initialize(enemyType, spawnPosition);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(spawnRadiusMin, spawnRadiusMax);
        return transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
    }
}