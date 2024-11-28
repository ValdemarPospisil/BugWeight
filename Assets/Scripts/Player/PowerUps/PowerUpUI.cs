using System.Collections.Generic;
using UnityEngine;

public class PowerUpUI : MonoBehaviour
{
    public Transform cardContainer; // Parent object for UI cards
    public GameObject cardPrefab; // Prefab for individual cards

    private PowerUpManager powerUpManager;

    private void Start()
    {
        powerUpManager = FindFirstObjectByType<PowerUpManager>();
    }

    public void DisplayChoices(List<PowerUp> powerUps)
    {
        ClearCards();

        foreach (PowerUp powerUp in powerUps)
        {
            GameObject card = Instantiate(cardPrefab, cardContainer);
            PowerUpCard cardScript = card.GetComponent<PowerUpCard>();
            cardScript.SetUp(powerUp, powerUpManager);
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
