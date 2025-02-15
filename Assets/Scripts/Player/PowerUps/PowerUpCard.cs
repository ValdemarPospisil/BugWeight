using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpCard : MonoBehaviour
{
    [SerializeField] private Image icon; // Icon image
    [SerializeField] private TextMeshProUGUI nameText; // Name of the power-up
    [SerializeField] private TextMeshProUGUI descriptionText; // Description of the power-up
    [SerializeField] private Button button; // Button to select the power-up

    private int showTier = 1;
    private PowerUp powerUp;
    private PowerUpUI powerUpUI;

    public void SetUp(PowerUp powerUpData, PowerUpUI ui)
    {
        powerUp = powerUpData;
        powerUpUI = ui;

        if (powerUp.basePicked)
        {
            showTier = powerUp.currentTier + 1;
        }
        else
        {
            showTier = powerUp.currentTier;
        }

        UpdateCardUI();
        button.onClick.AddListener(OnSelected);
    }

    private void UpdateCardUI()
    {
        if (showTier - 1 < powerUp.tierVariables.Count)
        {
            var tier = powerUp.tierVariables[showTier - 1];
            icon.sprite = powerUp.icon;
            nameText.text = $"{powerUp.baseName} {GetRomanNumeral(showTier)}";
            descriptionText.text = GenerateDescription(tier);
        }
    }

    private string GenerateDescription(TierVariable tier)
    {
        return $"{powerUp.baseDescription}\n" +
               $"Damage: {tier.damage}\n" +
               $"Speed: {tier.variable}\n";
    }

    private string GetRomanNumeral(int number)
    {
        switch (number)
        {
            case 1: return "I";
            case 2: return "II";
            case 3: return "III";
            case 4: return "IV";
            default: return number.ToString();
        }
    }

    private void OnSelected()
    {
        powerUp.basePicked = true;
        powerUpUI.OnPowerUpSelected(powerUp); // Notify PowerUpUI of the selection
    }
}