using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed of the character

    private Rigidbody2D rb;  // Reference to the Rigidbody2D component
    private Vector2 movement;  // Variable to store movement direction
    private Animator animator;
    private bool isMoving;
    [SerializeField]
    private float attackSpeed;
    [SerializeField]
    private float maxHP = 100f;
     [Header("UI Elements")]
     [SerializeField]
    private Image healthBar;       // Reference to the Health Bar Image
    [SerializeField]
    private TextMeshProUGUI healthText;     // Reference to the Health Text UI
    [SerializeField]
    private TextMeshProUGUI deathText;     // Reference to the Health Text UI
    private float currentHP;
    void Awake()
    {
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        // Start the automatic attack coroutine
       // StartCoroutine(AutoAttackRoutine());
       currentHP = maxHP;
       deathText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckInput();

        if (currentHP <= 0)
        {
            Death();
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
        // Update the health bar fill amount (assuming healthBar is set to Fill type)
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHP / maxHP;
        }

        // Update the health text to show the current HP
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
