using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public float currentXP { get; private set; }
    public int level { get; private set; } = 1;
    public float toNextLevel { get; private set; } = 100;

    private PlayerManager playerManager;
    private SpecialAbilityManager specialAbilityManager;
    private PowerUpManager powerUpManager;

    public PowerUpUI powerUpUI;
    public SpecialAbilityUI specialAbilityUI;

    public bool isChoosing = false; // Track if the player is currently choosing
    private int pendingLevelUps = 0; // Track how many level-ups are pending

    public void Start()
    {
        currentXP = 0;
        playerManager = ServiceLocator.GetService<PlayerManager>();
        specialAbilityManager = GetComponent<SpecialAbilityManager>();
        powerUpManager = GetComponent<PowerUpManager>();
        playerManager.UpdateUI();
        Invoke("ShowSpecialAbilityChoices", 0.65f);
    }

    public void AddXP(float amount)
    {
        currentXP += amount;
        currentXP = Mathf.Round(currentXP);
        CheckLevelUp();
        playerManager.UpdateUI();
    }

    private void ShowSpecialAbilityChoices()
    {
        List<SpecialAbility> specialAbilityChoices = specialAbilityManager.GetSpecialAbilityChoices();
        specialAbilityUI.DisplayChoices(specialAbilityChoices);
    }

    public void LevelUp()
    {
        pendingLevelUps++; // Increment pending level-ups
        ProcessLevelUp();
    }

    private void ProcessLevelUp()
    {
        if (pendingLevelUps > 0)
        {
            Time.timeScale = 0f;
            isChoosing = true;

            if (level % 5 == 0)
            {
                // Show special ability choices for every 5th level
                List<SpecialAbility> specialAbilityChoices = specialAbilityManager.GetSpecialAbilityChoices();
                specialAbilityUI.DisplayChoices(specialAbilityChoices);
            }
            else
            {
                // Show power-up choices for regular levels
                List<PowerUp> powerUpChoices = powerUpManager.GetPowerUpChoices();
                powerUpUI.DisplayChoices(powerUpChoices);
            }
        }
    }

    public void OnPowerUpSelected(PowerUp selectedPowerUp)
    {
        // Hide power-up UI
        powerUpUI.HideChoices();

        // Activate the selected power-up
        powerUpManager.ActivatePowerUp(selectedPowerUp);

        // Decrement pending level-ups
        pendingLevelUps--;

        // Resume time if no more pending level-ups
        if (pendingLevelUps <= 0)
        {
            Time.timeScale = 1f;
            isChoosing = false;
        }
        else
        {
            // Process the next level-up
            ProcessLevelUp();
        }

    }

    public void OnSpecialAbilitySelected(SpecialAbility selectedSpecialAbility)
    {
        // Hide special ability UI
        specialAbilityUI.HideChoices();

        // Activate the selected special ability
        specialAbilityManager.ActivateSpecialAbility(selectedSpecialAbility);

        // Decrement pending level-ups
        pendingLevelUps--;

        // Resume time if no more pending level-ups
        if (pendingLevelUps <= 0)
        {
            Time.timeScale = 1f;
            isChoosing = false;
        }
        else
        {
            // Process the next level-up
            ProcessLevelUp();
        }

        Debug.Log("Special ability selected: " + selectedSpecialAbility.name);
    }

    public void ResetLevel()
    {
        currentXP = 0;
        level = 1;
        toNextLevel = 100;
        if (playerManager != null)
        {
            playerManager.UpdateUI();
        }
        else
        {
            Debug.Log("Player manager is null");
        }
    }

    private void CheckLevelUp()
    {
        while (currentXP >= toNextLevel)
        {
            currentXP -= toNextLevel;
            level++;
            toNextLevel += toNextLevel * 0.2f; // Increment difficulty to level up
            toNextLevel = Mathf.Round(toNextLevel);
            playerManager.UpdateUI();
            LevelUp(); // Trigger level-up for each level gained
        }
    }
}