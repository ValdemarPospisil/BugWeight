using UnityEngine;
using System.Collections.Generic;

public class SpecialAbilityManager : MonoBehaviour
{
    public List<SpecialAbility> allAbilities;
    public List<SpecialAbility> activeAbilities = new List<SpecialAbility>();
    public SpecialAbilityUI specialAbilityUI; // Reference to the UI manager

    public void ShowSpecialAbilityChoices()
    {
        Time.timeScale = 0f;
        List<SpecialAbility> availableAbilities = new List<SpecialAbility>();

        if (activeAbilities.Count < 3)
        {
            // If the player has less than 3 abilities, show abilities they don't have or can upgrade
            foreach (var ability in allAbilities)
            {
                if (!activeAbilities.Contains(ability) || ability.CanUpgrade())
                {
                    availableAbilities.Add(ability);
                }
            }
        }
        else
        {
            // If the player has 3 abilities, only show abilities they can upgrade
            foreach (var ability in activeAbilities)
            {
                if (ability.CanUpgrade())
                {
                    availableAbilities.Add(ability);
                }
            }
        }

        List<SpecialAbility> randomSpecialAbilities = new List<SpecialAbility>();
        while (randomSpecialAbilities.Count < 3 && availableAbilities.Count > 0)
        {
            SpecialAbility randomSpecialAbility = availableAbilities[Random.Range(0, availableAbilities.Count)];
            if (!randomSpecialAbilities.Contains(randomSpecialAbility))
            {
                randomSpecialAbilities.Add(randomSpecialAbility);
                availableAbilities.Remove(randomSpecialAbility);
            }
        }
        specialAbilityUI.DisplayChoices(randomSpecialAbilities);
    }

    public void ResetSpecialAbilities()
    {
        foreach (var specialAbility in allAbilities)
        {
            specialAbility.currentTier = 1;
            specialAbility.basePicked = false;
        }
        activeAbilities.Clear();
        ShowSpecialAbilityChoices();
    }

    public void UseSpecialAbility(int index)
    {
        if (index < activeAbilities.Count)
        {
            activeAbilities[index]?.Activate();
        }
    }

    public void ActivateSpecialAbility(SpecialAbility ability)
    {
        Time.timeScale = 1f;
        if (!activeAbilities.Contains(ability))
        {
            if (activeAbilities.Count < 3)
            {
                activeAbilities.Add(ability);
            }
        }
        else
        {
            UpgradeSpecialAbility(ability);
        }
    }

    public void UpgradeSpecialAbility(SpecialAbility ability)
    {
        if (ability.CanUpgrade())
        {
            ability.Upgrade();
        }
    }
}