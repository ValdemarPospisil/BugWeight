using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public float currentXP { get; private set; }
    public int level { get; private set; } = 1;
    public float toNextLevel { get; private set; } = 100;

   // public UnityEvent OnLevelUp = new UnityEvent();
    public static LevelManager Instance { get; private set; }
    private PlayerManager playerManager;
    private SpecialAbilityManager specialAbilityManager;
    private PowerUpManager powerUpManager;

    public void Start()
    {
        currentXP = 0;
        playerManager = ServiceLocator.GetService<PlayerManager>();
        specialAbilityManager = GetComponent<SpecialAbilityManager>();
        powerUpManager = GetComponent<PowerUpManager>();
        playerManager.UpdateUI();
    }
    public void AddXP(float amount)
    {
        currentXP += amount;
        currentXP = Mathf.Round(currentXP);
        CheckLevelUp();
        playerManager.UpdateUI();
    }

    public void LevelUp () {
        if (level % 5 == 0) {
            specialAbilityManager.ShowSpecialAbilityChoices();
        }
        else
        {
            powerUpManager.ShowPowerUpChoices();
        }
    }

    public void ResetLevel()
    {
        currentXP = 0;
        level = 1;
        toNextLevel = 100;
        if (playerManager != null)
        {
            Debug.Log("Player manager is not null");
            playerManager.UpdateUI();
        }
        else
        {
            Debug.Log("Player manager is null");
        }
        
    }

    private void CheckLevelUp()
    {
        if (currentXP >= toNextLevel)
        {
            currentXP -= toNextLevel;
            level++;
            toNextLevel += toNextLevel * 0.2f; // Increment difficulty to level up
            toNextLevel = Mathf.Round(toNextLevel);
            playerManager.UpdateUI();
            LevelUp();
            //OnLevelUp.Invoke();
        }
    }
}
