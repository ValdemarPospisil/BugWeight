using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialAbilityCard : MonoBehaviour
{
    public Image icon; // Icon image
    public TextMeshProUGUI nameText; // Name of the special ability
    public TextMeshProUGUI descriptionText; // Description of the special ability
    public Button button; // Button to select the special ability

    private SpecialAbility specialAbility;
    private SpecialAbilityManager specialAbilityManager;

    public void SetUp(SpecialAbility abilityData, SpecialAbilityManager manager)
    {
        specialAbility = abilityData;
        specialAbilityManager = manager;

        icon.sprite = abilityData.icon; // Assuming the SpecialAbility class has an 'icon' property
        nameText.text = abilityData.abilityName;
        descriptionText.text = abilityData.abilityDescription;

        button.onClick.AddListener(OnSelected);
    }

    private void OnSelected()
    {   
        Debug.Log("Selected: " + specialAbility.abilityName);
        specialAbilityManager.ActivateSpecialAbility(specialAbility);
        specialAbilityManager.specialAbilityUI.HideChoices();
    }
}