using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]private List<Enemy> activeEnemies = new List<Enemy>();

    public void RegisterEnemy(Enemy enemy)
    {
        activeEnemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }

    private void Update()
    {
        foreach (Enemy enemy in activeEnemies)
        {
            if (enemy != null)
            {
               // enemy.UpdateEnemyLogic();
            }
        }
    }

    
}