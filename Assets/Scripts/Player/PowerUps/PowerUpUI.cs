using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    public Transform cardContainer; // Parent object for UI cards
    public GameObject cardPrefab; // Prefab for individual cards

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = ServiceLocator.GetService<LevelManager>();
    }

    public void DisplayChoices(List<PowerUp> powerUps)
    {
        ClearCards();
        int index = 0;
        foreach (PowerUp powerUp in powerUps)
        {
            GameObject card = Instantiate(cardPrefab, cardContainer);
            PowerUpCard cardScript = card.GetComponent<PowerUpCard>();
            cardScript.SetUp(powerUp, this);
            Button button = card.GetComponent<Button>();
            index++;
            if (index == 2)
            {
                button.Select();
            }
        }

        cardContainer.gameObject.SetActive(true);
    }

    public void OnPowerUpSelected(PowerUp selectedPowerUp)
    {
        // Notify LevelManager
        levelManager.OnPowerUpSelected(selectedPowerUp);
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