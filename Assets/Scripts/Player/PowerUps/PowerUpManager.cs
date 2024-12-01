using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<PowerUp> activePowerUps = new List<PowerUp>();
    public List<PowerUp> allPowerUps; // List of all available power-ups
    public PowerUpUI powerUpUI; // Reference to the UI manager

    private void Awake()
    {
        // Ensure this GameObject persists across scenes
        DontDestroyOnLoad(gameObject);
    }

    public void ShowPowerUpChoices()
    {
        Time.timeScale = 0f;
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
        Time.timeScale = 1f;
        if (activePowerUps.Contains(powerUp)) return;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        powerUp.Activate(player);
        activePowerUps.Add(powerUp);
    }

    public void DeactivatePowerUp(PowerUp powerUp)
    {
        if (activePowerUps.Contains(powerUp))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            powerUp.Deactivate(player);
            activePowerUps.Remove(powerUp);
        }
    }

    public void DeactivateAllPowerUps()
    {
        foreach (var powerUp in activePowerUps)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            powerUp.Deactivate(player);
        }
        activePowerUps.Clear();
    }
}