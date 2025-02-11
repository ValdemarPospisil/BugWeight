using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public float maxHP = 100f;
    public float currentHP { get; private set; }
    private int extraLives = 0;
    private float healthPercentageToRebirth = 0.5f;
    public delegate void FatalDamageHandler(Vector3 position);
    public event FatalDamageHandler OnFatalDamage;
    public static PlayerManager Instance { get; private set; }
    public static bool explodeOnDeath = false;
    [SerializeField] private GameObject explosionEffect; 
    public static float explosionRadius = 2f; // Radius of the explosion
    public static float explosionDamage;

    

    // UI elements
    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Image xpBar;
    [SerializeField] private TextMeshProUGUI levelText;
  //  [SerializeField] private TextMeshProUGUI xpText;
   // [SerializeField] private TextMeshProUGUI toNextLevelText;

    private LevelManager levelManager;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
        currentHP = maxHP;
        levelManager = ServiceLocator.GetService<LevelManager>();
    }

    private void Start()
    {
        UpdateUI();
        extraLives = 0;
        gameObject.SetActive(true);
        Time.timeScale = 1;
        healthPercentageToRebirth = 0.5f;
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
        Debug.Log("Health %:" + healthPercentage);
        healthPercentageToRebirth += healthPercentage;
        Debug.Log("Extra lives: " + extraLives + " " +  healthPercentageToRebirth);
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
            if (explodeOnDeath == true)
            {
                Explode();
            }
            deathScreen.gameObject.SetActive(true);
            gameObject.SetActive(false);
            PowerUpManager powerUpManager = ServiceLocator.GetService<PowerUpManager>();
            powerUpManager.DeactivateAllPowerUps();
            Time.timeScale = 0;
        }
    }
    private void Explode()
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            BloodExplosion explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity).GetComponent<BloodExplosion>();
            if (explosion != null)
            {
                explosion.SetUpExplosion(explosionDamage, explosionRadius, 0.8f);
            }
        }

    }
    private void Resurrect()
    {
        currentHP = maxHP * healthPercentageToRebirth;
        Debug.Log("Resurrected with " + currentHP + " HP");
        UpdateUI();
    }
}
