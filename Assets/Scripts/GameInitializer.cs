using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Register services
        ServiceLocator.RegisterService(FindFirstObjectByType<SpecialAbilityManager>());
        ServiceLocator.RegisterService(FindFirstObjectByType<PowerUpManager>());
        ServiceLocator.RegisterService(FindFirstObjectByType<LevelManager>());

        // Ensure this GameObject persists across scenes
        DontDestroyOnLoad(gameObject);
    }
}