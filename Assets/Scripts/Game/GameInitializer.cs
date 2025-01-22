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

        DontDestroyOnLoad(gameObject);
    }
}