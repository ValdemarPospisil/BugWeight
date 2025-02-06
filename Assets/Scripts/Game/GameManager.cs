using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SpecialAbilityManager specialAbilityManager;
    public PowerUpManager powerUpManager;
    public LevelManager levelManager;
    public TargetingSystem targetingSystem;
    public ProjectileFactory projectileFactory;
    public EnemySpawner enemySpawner;
    public PlayerManager playerManager;
    public PlayerController playerController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       // StartNewGame();
    }
    private void Start()
    {
        levelManager.ResetLevel();
        enemySpawner.ResetEnemies();
        powerUpManager.ResetPowerUps();
        specialAbilityManager.ResetSpecialAbilities();
        playerManager.ResetPlayer();
        playerManager.transform.position = new Vector3(85, 85, 0);
    }


    public void StartNewGame()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
         /*
        Time.timeScale = 1;
        playerManager.gameObject.SetActive(true);
        
        
        
        
        
        */

    }
}