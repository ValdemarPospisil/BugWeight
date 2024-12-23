using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    public Transform cardContainer; // Parent object for UI cards
    public GameObject cardPrefab; // Prefab for individual cards

    private PowerUpManager powerUpManager;

    private void Start()
    {
        powerUpManager = ServiceLocator.GetService<PowerUpManager>();
    }

    public void DisplayChoices(List<PowerUp> powerUps)
    {
        ClearCards();

        foreach (PowerUp powerUp in powerUps)
        {
            GameObject card = Instantiate(cardPrefab, cardContainer);
            PowerUpCard cardScript = card.GetComponent<PowerUpCard>();
            cardScript.SetUp(powerUp, powerUpManager);
            Button button = card.GetComponent<Button>();
            button.Select();
        }

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