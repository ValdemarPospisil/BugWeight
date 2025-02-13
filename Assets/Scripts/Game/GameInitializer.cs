using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.RegisterService(FindFirstObjectByType<SpecialAbilityManager>());
        ServiceLocator.RegisterService(FindFirstObjectByType<PowerUpManager>());
        ServiceLocator.RegisterService(FindFirstObjectByType<LevelManager>());
        ServiceLocator.RegisterService(FindFirstObjectByType<TargetingSystem>());
        ServiceLocator.RegisterService(FindFirstObjectByType<ProjectileFactory>());
        ServiceLocator.RegisterService(FindFirstObjectByType<EnemySpawner>());
        ServiceLocator.RegisterService(FindFirstObjectByType<PlayerManager>());
        ServiceLocator.RegisterService(FindFirstObjectByType<PlayerController>());
        ServiceLocator.RegisterService(FindFirstObjectByType<KillCounter>());

       // DontDestroyOnLoad(gameObject);
    }
}