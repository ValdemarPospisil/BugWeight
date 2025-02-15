using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<PowerUp> activePowerUps = new List<PowerUp>();
    public List<PowerUp> allPowerUps; // List of all available power-ups

    public List<PowerUp> GetPowerUpChoices()
    {
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

        return randomPowerUps;
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
            powerUp.Deactivate();
            activePowerUps.Remove(powerUp);
        }
    }

    public void DeactivateAllPowerUps()
    {
        foreach (var powerUp in activePowerUps)
        {
            powerUp.Deactivate();
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