using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed of the character

    private Rigidbody2D rb;  // Reference to the Rigidbody2D component
    private Vector2 movement;  // Variable to store movement direction
    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    private Animator animator;
    private bool isAttacking;
    [SerializeField]
    private float attackSpeed;
    void Awake()
    {
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Get the SpriteRenderer component attached to the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // Start the automatic attack coroutine
       // StartCoroutine(AutoAttackRoutine());
    }

    void Update()
    {
        // Get input from horizontal (A/D or Left/Right arrow) and vertical (W/S or Up/Down arrow) axes
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector to prevent faster diagonal movement
        movement = movement.normalized;

        // Flip the sprite based on the direction of movement
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;  // Facing right
            

        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;   // Facing left
        }
        
        if (animator != null)
        {
            if(movement != Vector2.zero)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
        }
        rb.linearVelocity = movement * moveSpeed;
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
