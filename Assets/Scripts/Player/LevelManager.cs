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
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        specialAbilityManager = GetComponent<SpecialAbilityManager>();
        powerUpManager = GetComponent<PowerUpManager>();
        playerManager.UpdateUI();
    }
    public void AddXP(float amount)
    {
        currentXP += amount;
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

    private void CheckLevelUp()
    {
        if (currentXP >= toNextLevel)
        {
            currentXP -= toNextLevel;
            level++;
            toNextLevel += toNextLevel * 0.3f; // Increment difficulty to level up
            playerManager.UpdateUI();
            LevelUp();
            //OnLevelUp.Invoke();
        }
    }
}
