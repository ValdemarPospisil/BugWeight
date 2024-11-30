using UnityEngine;
using System.Collections.Generic;

public class SpecialAbilityManager : MonoBehaviour
{
    public List<SpecialAbility> allAbilities;
    private SpecialAbility activeAbility;
    public SpecialAbilityUI specialAbilityUI; // Reference to the UI manager

    private void Start()
    {
        ShowSpecialAbilityChoices(); // Randomly select 3 abilities for the player
    }

    public void ShowSpecialAbilityChoices()
    {
        // Randomly select 3 special abilities
        List<SpecialAbility> randomSpecialAbilities = new List<SpecialAbility>();
        while (randomSpecialAbilities.Count < 3)
        {
            SpecialAbility randomSpecialAbility = allAbilities[Random.Range(0, allAbilities.Count)];
            if (!randomSpecialAbilities.Contains(randomSpecialAbility))
            {
                randomSpecialAbilities.Add(randomSpecialAbility);
            }
        }
        specialAbilityUI.DisplayChoices(randomSpecialAbilities);
    }

    public void UseSpecialAbility()
    {
        activeAbility?.Activate();
    }

    public void ActivateSpecialAbility(SpecialAbility ability)
    {
        activeAbility = ability;
        //UseSpecialAbility();
    }
}