using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour, IDamageable
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

    void Start()
    {
        
        currentHP = maxHP;
        UpdateUI();
    }

    void Update()
    {
        
        if (currentHP <= 0) Death();
        if (currentXP >= toNextLevel) LevelUp();
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

    public void TakeDamage(float amount)
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
