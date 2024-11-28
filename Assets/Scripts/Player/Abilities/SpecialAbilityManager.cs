using UnityEngine;
using System.Collections.Generic;

public class SpecialAbilityManager : MonoBehaviour
{
    public List<SpecialAbility> allAbilities;
    private SpecialAbility activeAbility;

    private void Start()
    {
        SelectRandomAbilities(3); // Randomly select 3 abilities for the player
    }

    private void SelectRandomAbilities(int count)
    {
        var chosenAbilities = new List<SpecialAbility>();
        for (int i = 0; i < count; i++)
        {
            SpecialAbility ability = allAbilities[Random.Range(0, allAbilities.Count)];
            if (!chosenAbilities.Contains(ability)) chosenAbilities.Add(ability);
        }

        // Present choices to the player (UI logic here)
        activeAbility = chosenAbilities[0]; // Example: Pick the first for now
    }

    public void UseSpecialAbility()
    {
        activeAbility?.Activate();
    }
}
