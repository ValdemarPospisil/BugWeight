using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public float maxHP = 100f;
    public float currentHP { get; private set; }
    private int extraLives = 0;
    private float healthPercentageToRebirth = 0.5f;
    public delegate void FatalDamageHandler(Vector3 position);
    public event FatalDamageHandler OnFatalDamage;
    

    // UI elements
    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private Image xpBar;
    [SerializeField] private TextMeshProUGUI levelText;
  //  [SerializeField] private TextMeshProUGUI xpText;
   // [SerializeField] private TextMeshProUGUI toNextLevelText;

    [SerializeField] private GameObject deathParticles;
    private LevelManager levelManager;

    private void Awake() {
        currentHP = maxHP;
        levelManager = ServiceLocator.GetService<LevelManager>();
    }

    private void Start()
    {
        UpdateUI();
        extraLives = 0;
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

    public void BuffHealth(float amount)
    {
        maxHP += amount;
        if (amount > 0)
        {
            currentHP += amount;
        }
        if (currentHP > maxHP) currentHP = maxHP;
        UpdateUI();
    }

    public void AddExtraLives(int lives, float healthPercentage)
    {
        extraLives += lives;
        healthPercentageToRebirth = healthPercentage;
        Debug.Log("Player received " + lives + " extra lives and " + healthPercentage * 100 + "% health on resurrection.");
    }

    public void UpdateUI()
    {
        healthBar.fillAmount = currentHP / maxHP;
        healthText.text = Mathf.Ceil(currentHP).ToString();
        xpBar.fillAmount = levelManager.currentXP / levelManager.toNextLevel;
        levelText.text = levelManager.level.ToString();
    //    xpText.text = levelManager.currentXP.ToString();
    //    toNextLevelText.text = levelManager.toNextLevel.ToString();
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
        OnFatalDamage?.Invoke(transform.position);

        if (extraLives > 0)
        {
            extraLives--;
            Resurrect();
        }
        else
        {
            deathParticles = Instantiate(deathParticles, transform.position, Quaternion.identity);
            deathText.gameObject.SetActive(true);
            Time.timeScale = 0;
            Destroy(gameObject);
        }
    }

    private void Resurrect()
    {
        currentHP = maxHP * healthPercentageToRebirth;
        UpdateUI();
        Debug.Log("Player resurrected with " + currentHP + " HP.");
    }
}
