using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<PowerUp> activePowerUps = new List<PowerUp>();
    public List<PowerUp> allPowerUps; // List of all available power-ups
    public PowerUpUI powerUpUI; // Reference to the UI manager

    private void Start()
    {
        ShowPowerUpChoices();
    }

    public void ShowPowerUpChoices()
    {
        // Randomly select 3 power-ups
        List<PowerUp> randomPowerUps = new List<PowerUp>();
        while (randomPowerUps.Count < 3)
        {
            PowerUp randomPowerUp = allPowerUps[Random.Range(0, allPowerUps.Count)];
            if (!randomPowerUps.Contains(randomPowerUp))
            {
                randomPowerUps.Add(randomPowerUp);
            }
        }
        powerUpUI.DisplayChoices(randomPowerUps);
    }

    public void ActivatePowerUp(PowerUp powerUp)
    {
        // Prevent duplicate power-ups
        if (activePowerUps.Contains(powerUp)) return;

        // Activate and add to list
        powerUp.Activate(gameObject);
        activePowerUps.Add(powerUp);
    }

    public void DeactivatePowerUp(PowerUp powerUp)
    {
        if (activePowerUps.Contains(powerUp))
        {
            powerUp.Deactivate(gameObject);
            activePowerUps.Remove(powerUp);
        }
    }

    public void DeactivateAllPowerUps()
    {
        foreach (var powerUp in activePowerUps)
        {
            powerUp.Deactivate(gameObject);
        }
        activePowerUps.Clear();
    }
}
