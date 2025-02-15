using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialAbilityCard : MonoBehaviour
{
    public Image icon; // Icon image
    public TextMeshProUGUI nameText; // Name of the special ability
    public TextMeshProUGUI descriptionText; // Description of the special ability
    public Button button; // Button to select the special ability

    private int showTier = 1;
    private SpecialAbility specialAbility;
    private SpecialAbilityUI specialAbilityUI;

    public void SetUp(SpecialAbility abilityData, SpecialAbilityUI ui)
    {
        specialAbility = abilityData;
        specialAbilityUI = ui;

        if (specialAbility.basePicked)
        {
            showTier = specialAbility.currentTier + 1;
        }
        else
        {
            showTier = specialAbility.currentTier;
        }

        UpdateCardUI();
        button.onClick.AddListener(OnSelected);
    }

    private void UpdateCardUI()
    {
        if (showTier < specialAbility.maxTier)
        {
            icon.sprite = specialAbility.icon;
            nameText.text = $"{specialAbility.name} {GetRomanNumeral(showTier)}";
            descriptionText.text = specialAbility.abilityDescription;
        }
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
        specialAbility.basePicked = true;
        specialAbilityUI.OnSpecialAbilitySelected(specialAbility); // Notify SpecialAbilityUI of the selection
    }
}