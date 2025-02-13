using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

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
    public KillCounter killCounter;
    [SerializeField] private CanvasGroup fadePanel; // Assign the Panel's Canvas Group
    [SerializeField] private float fadeDuration = 1f; // Time for fade

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
        powerUpManager.ResetPowerUps();
        specialAbilityManager.ResetSpecialAbilities();
        playerManager.transform.position = new Vector3(85, 85, 0);
    }

    public void StartNewGame()
    {
        StartCoroutine(FadeOutAndReload());
    }

    IEnumerator FadeIn()
    {
        fadePanel.alpha = 1; // Start fully black
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        fadePanel.alpha = 0; // Ensure it's fully transparent
    }

    IEnumerator FadeOutAndReload()
    {
        float elapsedTime = 0;
        fadePanel.alpha = 0; // Start fully transparent

        while (elapsedTime < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        fadePanel.alpha = 1; // Ensure it's fully opaque
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
