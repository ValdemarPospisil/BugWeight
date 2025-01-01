using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies; // List of ScriptableObject data assets

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
        enemyCount += Random.Range(0, levelManager.level * 2);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject randomEnemies = enemies[Random.Range(0, enemies.Count)];
            Vector3 spawnPosition = GetRandomSpawnPosition();
            SimpleEnemy enemy = Instantiate(randomEnemies, spawnPosition, Quaternion.identity).GetComponent<SimpleEnemy>();
            enemy.Initialize(spawnPosition);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(spawnRadiusMin, spawnRadiusMax);
        return transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * distance;
    }
}