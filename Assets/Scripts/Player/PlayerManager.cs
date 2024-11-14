using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public float maxHP = 100f;
    public float currentHP;
    public float currentXP;
    private int level = 1;
    private float toNextLevel = 100;

    // UI elements
    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private Image xpBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI toNextLevelText;

    // Player class management
    private PlayerClass activeClass;
    public List<PlayerClass> playerClasses; // Assign Warrior, Hunter, etc., in Inspector

    private void Start()
    {
        // Initialize with the first class as default or any class you prefer
        SetClass(playerClasses[0]);
        currentHP = maxHP;
        UpdateUI();
    }

    private void Update()
    {
        // Example for switching classes for testing (can remove in final)
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetClass(playerClasses[0]); // Warrior
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetClass(playerClasses[1]); // Hunter
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetClass(playerClasses[2]); // Wizard
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetClass(playerClasses[3]); // Vampire

        if (currentHP <= 0) Death();
        if (currentXP >= toNextLevel) LevelUp();
    }

    public void SetClass(PlayerClass newClass)
    {
        if (activeClass != null) activeClass.gameObject.SetActive(false);
        activeClass = newClass;
        activeClass.gameObject.SetActive(true);
       // activeClass.Initialize(this); // Pass reference to PlayerManager
    }

    private void LevelUp()
    {
        level++;
        currentXP = 0;
        toNextLevel += toNextLevel * 0.3f;
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthBar.fillAmount = currentHP / maxHP;
        healthText.text = Mathf.Ceil(currentHP).ToString();
        xpBar.fillAmount = currentXP / toNextLevel;
        levelText.text = level.ToString();
        xpText.text = currentXP.ToString();
        toNextLevelText.text = toNextLevel.ToString();
    }

    public void DamagePlayer(float amount)
    {
        currentHP -= amount;
        UpdateUI();
    }

    private void Death()
    {
        Debug.Log("The Player is dead!");
        deathText.gameObject.SetActive(true);
    }
}
