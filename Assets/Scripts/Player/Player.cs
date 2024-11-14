using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; 

    private Rigidbody2D rb;  
    private Vector2 movement;  
    private bool isMoving;
    private int level;
    private float toNextLevel;
    public float currentXP;
    private float currentHP;
    private Animator activeAnimator;
    private SpriteRenderer spriteRenderer;
    



    [Header("Player Stats")]
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float maxHP = 100f;
    [SerializeField]
    private string activeClass = "Wizard"; 


    [Header("Animators")]
    [SerializeField]
    private Animator meleeAnimator;
    [SerializeField]
    private Animator rangedAnimator;
    [SerializeField]
    private Animator magicAnimator;
    [SerializeField]
    private Animator vampirecAnimator;
    

    
    
    [Header("UI Elements")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI deathText;

    [SerializeField] private Image xpBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI xpText;
    [SerializeField] private TextMeshProUGUI toNextLevelText;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateActiveAnimator();
    }

    void Start()
    {
       currentHP = maxHP;
       deathText.gameObject.SetActive(false);
       level = 1;
       toNextLevel = 100;
       currentXP = 0;
       UpdateLevelUI();
       UpdateHealthUI();
    }

    private void Update()
    {
        CheckInput();
        HandleCombatInput();
        UpdateHealthUI();

        if (currentHP <= 0)
        {
            Death();
        }

        if (currentXP >= toNextLevel)
        {
            LevelUp();
        }
    }

    private void UpdateActiveAnimator()
    {
        // Set activeAnimator based on class
        meleeAnimator.gameObject.SetActive(activeClass == "Warrior");
        rangedAnimator.gameObject.SetActive(activeClass == "Hunter");
        magicAnimator.gameObject.SetActive(activeClass == "Wizard");
        vampirecAnimator.gameObject.SetActive(activeClass == "Vampire");

        activeAnimator = activeClass == "Warrior" ? meleeAnimator :
                         activeClass == "Hunter" ? rangedAnimator :
                         activeClass == "Vampire" ? vampirecAnimator :
                         magicAnimator;
    }

    private void LevelUp () {
        level =+ 1;
        currentXP = 0;
        toNextLevel = toNextLevel + (toNextLevel * 0.3f); 
        UpdateLevelUI();
    }


    public void UpdateLevelUI()
    {
       
        if (xpBar != null)
        {
            xpBar.fillAmount = currentXP / toNextLevel;
        }

        if (levelText != null)
        {
            levelText.text = Mathf.Ceil(level).ToString();
        }
        if (xpText != null)
        {
            xpText.text = Mathf.Ceil(currentXP).ToString();
        }
        if (toNextLevelText != null)
        {
            toNextLevelText.text = Mathf.Ceil(toNextLevel).ToString();
        }
    }

    private void CheckInput()
{
    movement.x = Input.GetAxisRaw("Horizontal");
    movement.y = Input.GetAxisRaw("Vertical");

    // Normalize movement to avoid faster diagonal movement
    movement = movement.normalized;

    // Set base animator parameters for movement and idle states
    isMoving = movement != Vector2.zero;
    /*
    animator.SetBool("Idle", !isMoving);

    if (isMoving)
    {
        animator.SetFloat("MovementX", movement.x);
        animator.SetFloat("MovementY", movement.y);
    }
    */
    // Set active weapon animator parameters, if applicable
    if (activeAnimator != null)
    {
        activeAnimator.SetBool("Idle", !isMoving);

        if (isMoving)
        {
            activeAnimator.SetFloat("MovementX", movement.x);
            activeAnimator.SetFloat("MovementY", movement.y);
        }
    }
 
    // Move the player by updating the rigidbody's velocity
    rb.linearVelocity = movement * moveSpeed;
}

    private void HandleCombatInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && activeAnimator != null)
        {
            if (activeClass == "Warrior")
                activeAnimator.SetTrigger("Melee");
            else if (activeClass == "Hunter")
                activeAnimator.SetTrigger("Ranged");
            else if (activeClass == "Wizard")
                activeAnimator.SetTrigger("Magic");
        }
    }
 


      public void DamagePlayer(float amount)
    {
        currentHP -= amount;
        Debug.Log("Player damaged! Current HP: " + currentHP);
        UpdateHealthUI();
    }


    private void UpdateHealthUI()
    {
       
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHP / maxHP;
        }

        
        if (healthText != null)
        {
            healthText.text = Mathf.Ceil(currentHP).ToString();
        }
    }

    private void Death () {
        Debug.Log("The Player is dead!");
        gameObject.SetActive(false);
        deathText.gameObject.SetActive(true);
    }


    
}
