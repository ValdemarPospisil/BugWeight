using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Movement speed of the character

    private Rigidbody2D rb;  // Reference to the Rigidbody2D component
    private Vector2 movement;  // Variable to store movement direction
    private SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component

    void Start()
    {
        // Get the Rigidbody2D component attached to the GameObject
        rb = GetComponent<Rigidbody2D>();

        // Get the SpriteRenderer component attached to the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
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
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        // Move the character by setting its velocity
        rb.velocity = movement * moveSpeed;
    }
}
