using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpCard : MonoBehaviour
{
    public Image icon; // Icon image
    public TextMeshProUGUI nameText; // Name of the power-up
    public TextMeshProUGUI descriptionText; // Description of the power-up
    public Button button; // Button to select the power-up

    private PowerUp powerUp;
    private PowerUpManager powerUpManager;

    public void SetUp(PowerUp powerUpData, PowerUpManager manager)
    {
        powerUp = powerUpData;
        powerUpManager = manager;

        icon.sprite = powerUpData.icon; // Assuming the PowerUp class has an 'icon' property
        nameText.text = powerUpData.skillName;
        descriptionText.text = powerUpData.skillDescription;

        button.onClick.AddListener(OnSelected);
    }

    private void OnSelected()
    {
        powerUpManager.ActivatePowerUp(powerUp);
        powerUpManager.powerUpUI.HideChoices();
    }
}
