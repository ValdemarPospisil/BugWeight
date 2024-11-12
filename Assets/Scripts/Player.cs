using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; 

    private Rigidbody2D rb;  
    private Vector2 movement;  
    private Animator animator;
    private bool isMoving;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float maxHP = 100f;
     [Header("UI Elements")]
     [SerializeField]
    private Image healthBar;      
    [SerializeField]
    private TextMeshProUGUI healthText;    
    [SerializeField]
    private TextMeshProUGUI deathText;    
    private float currentHP;

     [SerializeField]
    private Image xpBar;      
    [SerializeField]
    private TextMeshProUGUI levelText;     
    [SerializeField]
    private TextMeshProUGUI xpText;    
    [SerializeField]
    private TextMeshProUGUI toNextLevelText;    

    private int level;
    private float toNextLevel;
    public float currentXP;


    void Awake()
    {
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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

    void Update()
    {
        CheckInput();

        
    }

    private void FixedUpdate() {
        if (currentHP <= 0)
        {
            Death();
        }
        if (currentXP >= toNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp () {
        level =+ 1;
        currentXP = 0;
        toNextLevel = toNextLevel + (toNextLevel * 0.3f); 
        UpdateLevelUI();
    }


    private void UpdateLevelUI()
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
        // Get input from horizontal (A/D or Left/Right arrow) and vertical (W/S or Up/Down arrow) axes
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector to prevent faster diagonal movement
        movement = movement.normalized;
        if(movement != Vector2.zero)
        {
            isMoving = true;
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Idle", true);
        }
        

        if(isMoving == true)
        {
            
            
            if (movement.x > 0)
            {
                animator.SetFloat("MovementX", 1);
            }
            else if (movement.x < 0)
            {
                animator.SetFloat("MovementX", -1);
            }
            else
            {
                animator.SetFloat("MovementX", 0);
            }
            
            if (movement.y > 0)
            {
                animator.SetFloat("MovementY", 1);
            }
            else if (movement.y < 0)
            {
                animator.SetFloat("MovementY", -1);
            }
            else
            {
                animator.SetFloat("MovementY", 0);
            }
            
            
            
        }

        rb.linearVelocity = movement * moveSpeed;
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


    private IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed); // Wait 5 seconds before each attack
            TriggerAttack();
        }
    }

    private void TriggerAttack()
    {
        // Trigger the attack animation
        animator.SetTrigger("isAttacking");
    }
    
}
