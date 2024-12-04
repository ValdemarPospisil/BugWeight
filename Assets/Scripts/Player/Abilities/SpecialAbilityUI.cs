using System.Collections.Generic;
using UnityEngine;

public class SpecialAbilityUI : MonoBehaviour
{
    [SerializeField] private Transform cardContainer; // Parent object for UI cards
    [SerializeField] private GameObject cardPrefab; // Prefab for individual cards

    private SpecialAbilityManager specialManager;

    private void Start()
    {
        specialManager = ServiceLocator.GetService<SpecialAbilityManager>();
        if (specialManager == null)
        {
            Debug.LogError("SpecialAbilityManager not found in ServiceLocator.");
        }
    }

    public void DisplayChoices(List<SpecialAbility> abilities)
    {
        ClearCards();

        foreach (SpecialAbility ability in abilities)
        {
            GameObject card = Instantiate(cardPrefab, cardContainer);
            SpecialAbilityCard cardScript = card.GetComponent<SpecialAbilityCard>();
            cardScript.SetUp(ability, specialManager);
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