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
     //   DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ShowPowerUpChoices();
    }

    public void ShowPowerUpChoices()
    {
        Time.timeScale = 0f;
        List<PowerUp> availablePowerUps = GetAvailablePowerUps();
        List<PowerUp> randomPowerUps = new List<PowerUp>();

        for (int i = 0; i < availablePowerUps.Count; i++)
        {
            PowerUp temp = availablePowerUps[i];
            int randomIndex = Random.Range(i, availablePowerUps.Count);
            availablePowerUps[i] = availablePowerUps[randomIndex];
            availablePowerUps[randomIndex] = temp;
        }

        for (int i = 0; i < Mathf.Min(3, availablePowerUps.Count); i++)
        {
            randomPowerUps.Add(availablePowerUps[i]);
        }

        powerUpUI.DisplayChoices(randomPowerUps);
    }

    private List<PowerUp> GetAvailablePowerUps()
    {
        List<PowerUp> availablePowerUps = new List<PowerUp>();

        foreach (PowerUp powerUp in allPowerUps)
        {
            if (activePowerUps.Contains(powerUp))
            {
                if (powerUp.CanUpgrade())
                {
                    availablePowerUps.Add(powerUp);
                }
            }
            else
            {
                availablePowerUps.Add(powerUp);
            }
        }

        return availablePowerUps;
    }

    public void ActivatePowerUp(PowerUp powerUp)
    {
        Time.timeScale = 1f;
        if (activePowerUps.Contains(powerUp))
        {
            powerUp.Upgrade();
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            powerUp.Activate(player);
            activePowerUps.Add(powerUp);
        }
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

    public void ResetPowerUps()
    {
        foreach (var powerUp in allPowerUps)
        {
            powerUp.currentTier = 1;
            powerUp.basePicked = false;
        }
    }
}