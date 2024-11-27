using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Rendering;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; 
    [SerializeField] private float baseMoveSpeed = 5f;

    private PlayerClass activeClass;    
    public Vector2 movementInput;
    public bool isMoving {get; private set; }

    private Animator animator;

    public List<PlayerClass> playerClasses; 
    public Vector2 lastDirection {get; private set; }
    private bool canMove;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        SetClass(playerClasses[0]);
        
        animator = GetComponent<Animator>();

        lastDirection = Vector2.right;
    }

    private void Start()
    {
        if (activeClass != null)
        {
            animator.runtimeAnimatorController = activeClass.animatorController;
        }
        else
        {
            Debug.LogError("No active PlayerClass found in PlayerManager!");
        }
        canMove = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && activeClass != null)
        {
            activeClass.StopAttack();
            activeClass.isAttacking = false; // Stop attacking
            animator.SetBool("IsAttacking", false);
            canMove = true; 

            
        }
        HandleInput();

    }

    private void Attack()
    {
        activeClass.Attack();
    }
    private void StopAttack()
    {
        activeClass.StopAttack();
        Debug.Log("Stopped attacking");
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
    
    if (Input.GetKeyDown(KeyCode.Space) && activeClass != null)
    {
        rb.linearVelocity = Vector2.zero; // Stop moving
        canMove = false; // Prevent movement
        activeClass.isAttacking = false; // Start attacking
        animator.SetBool("IsAttacking", true);
        //activeClass.Attack();
    }
    
    

    if (Input.GetKeyDown(KeyCode.E) && activeClass != null)
    {
        activeClass.Special();
    }

    if (!activeClass.isAttacking)
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
        movementInput = movementInput.normalized;

        isMoving = movementInput != Vector2.zero;
    }

    if (isMoving)
    {
        lastDirection = movementInput;
    }

    animator.SetBool("Idle", !isMoving);

    if (isMoving)
    {
        animator.SetFloat("MovementX", movementInput.x);
        animator.SetFloat("MovementY", movementInput.y);
    }
}

    private void MovePlayer()
    {
        if (activeClass != null && canMove)
        {
            rb.linearVelocity = movementInput * activeClass.GetMoveSpeed(baseMoveSpeed);
        }
    }

    private void CanMove()
    {
        canMove = true;
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
