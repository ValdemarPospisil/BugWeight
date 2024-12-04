using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SpecialAbilityManager specialAbilityManager;
    public PowerUpManager powerUpManager;
    public LevelManager levelManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Initialize game state
        StartNewGame();
    }

    public void StartNewGame()
    {
        powerUpManager.ResetPowerUps();
        // Reset other game states as needed
    }
}