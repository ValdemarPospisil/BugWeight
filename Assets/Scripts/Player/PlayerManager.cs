using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public float maxHP = 100f;
    public float currentHP { get; private set; }
    

    // UI elements
    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private Image xpBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI toNextLevelText;

    [SerializeField] private GameObject deathParticles;
    private LevelManager levelManager;

    private void Awake() {
        currentHP = maxHP;
        levelManager = ServiceLocator.GetService<LevelManager>();
    }

    void Update()
    {
        
        if (currentHP <= 0) Death();
    }

    public void Heal (float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthBar.fillAmount = currentHP / maxHP;
        healthText.text = Mathf.Ceil(currentHP).ToString();
        xpBar.fillAmount = levelManager.currentXP / levelManager.toNextLevel;
        levelText.text = levelManager.level.ToString();
        xpText.text = levelManager.currentXP.ToString();
        toNextLevelText.text = levelManager.toNextLevel.ToString();
    }
    
    public float GetHealthPercentage()
    {
        return currentHP / maxHP;
    }
    
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        UpdateUI();
    }

    private void Death()
    {
        deathParticles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        deathText.gameObject.SetActive(true);
        Time.timeScale = 0;
        Destroy(gameObject);
    }
}
