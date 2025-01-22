using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public SpecialAbilityManager specialAbilityManager;
    public PowerUpManager powerUpManager;
    public LevelManager levelManager;
    public TargetingSystem targetingSystem;
    public ProjectileFactory projectileFactory;

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
    }
    private void Start()
    {
        StartNewGame();
    }


    public void StartNewGame()
    {
        powerUpManager.ResetPowerUps();
        specialAbilityManager.ResetSpecialAbilities();
    }
}