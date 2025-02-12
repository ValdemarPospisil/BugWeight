using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAbilityUI : MonoBehaviour
{
    [SerializeField] private Transform cardContainer; // Parent object for UI cards
    [SerializeField] private GameObject cardPrefab; // Prefab for individual cards


    public void DisplayChoices(List<SpecialAbility> abilities)
    {
        ClearCards();
        int index = 0;
        foreach (SpecialAbility ability in abilities)
        {
            GameObject card = Instantiate(cardPrefab, cardContainer);
            SpecialAbilityCard cardScript = card.GetComponent<SpecialAbilityCard>();
            cardScript.SetUp(ability);
            Button button = card.GetComponent<Button>();
            index++;
            if (index == 2)
            {
                button.Select();
            }
        }

        // Show the card container (if hidden by default)
        cardContainer.gameObject.SetActive(true);
    }

    public void HideChoices()
    {
        cardContainer.gameObject.SetActive(false);
    }

    private void ClearCards()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }
    }
}