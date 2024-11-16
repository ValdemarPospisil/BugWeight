using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;   // Reference to Rigidbody2D
    [SerializeField] private float baseMoveSpeed = 5f; // Default movement speed

    private PlayerClass activeClass;     // The currently active class
    private Vector2 movementInput;      // Movement input
    private bool isMoving;

    private Animator animator;

    public List<PlayerClass> playerClasses; // Assign Warrior, Hunter, etc., in Inspector


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        SetClass(playerClasses[0]);
        
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Get the active class from PlayerManager
        if (activeClass != null)
        {
            animator.runtimeAnimatorController = activeClass.animatorController;
        }
        else
        {
            Debug.LogError("No active PlayerClass found in PlayerManager!");
        }
        
    }

    private void Update()
    {
        HandleInput();

        // Handle Attack Input
        if (Input.GetKeyDown(KeyCode.Space) && activeClass != null)
        {
            activeClass.Attack();
            animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void SetClass(PlayerClass newClass)
    {
        if (activeClass != null) activeClass.gameObject.SetActive(false);
        activeClass = newClass;
        activeClass.gameObject.SetActive(true);
        Debug.Log(activeClass);
        //activeClass.Initialize(this); // Pass reference to PlayerManager
    }

   
    private void HandleInput()
    {
        // Capture movement input
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput = movementInput.normalized;

        isMoving = movementInput != Vector2.zero;


        if (animator != null)
        {
            animator.SetBool("Idle", !isMoving);

            if (isMoving)
            {
                animator.SetFloat("MovementX", movementInput.x);
                animator.SetFloat("MovementY", movementInput.y);
            }
        }
        else
        {
            Debug.LogError("Animator is null");
        }
 

        // Normalize input to avoid faster diagonal movement
        
    }

    private void MovePlayer()
    {
        if (activeClass != null)
        {
            float moveSpeed = activeClass.GetMoveSpeed(baseMoveSpeed);
            rb.linearVelocity = movementInput * moveSpeed; // Note: `velocity` instead of `linearVelocity`
        }
    }

    
    public void SwitchClass(PlayerClass newClass)
    {
        if (newClass == null)
        {
            Debug.LogError("Attempted to switch to a null PlayerClass!");
            return;
        }

        activeClass = newClass;
        animator.runtimeAnimatorController = activeClass.animatorController;
        Debug.Log("Switched to class: " + activeClass.GetType().Name);
    }
}
