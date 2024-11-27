using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Wizard : PlayerClass
{
    public static event System.Action OnPlayerInvisible;

    [Header("Wizard-Specific Settings")]
    [SerializeField] private GameObject beam; 
    [SerializeField] private GameObject clonePrefab; // Clone decoy prefab
    [SerializeField] private float invisibilityDuration = 5f; // Duration of invisibility
    
    private SpriteRenderer spriteRenderer;
    private Vector2 direction;

    private void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        beam.SetActive(false); // Ensure the beam is initially inactive
    }

    public override void Attack()
    {
        // Set the beam active
        beam.SetActive(true);

        // Calculate the direction based on the player's movement input
        
    }

    public override void StopAttack()
    {
        // Set the beam inactive
        beam.SetActive(false);
    }

    public override void Special()
    {
        Debug.Log("Wizard uses Clone Decoy and turns invisible.");
        StartCoroutine(CloneDecoy());
    }

    private IEnumerator CloneDecoy()
    {
        if (clonePrefab == null)
        {
            Debug.LogError("Clone prefab is not assigned!");
            yield break;
        }
        // Create the decoy clone
        GameObject clone = Instantiate(clonePrefab, transform.position, Quaternion.identity);
        clone.GetComponent<Health>().SetHealth(maxHP); // Transfer current HP to clone

        transform.parent.gameObject.tag = "Invisible";        
        OnPlayerInvisible?.Invoke();

        // Make the wizard invisible
        spriteRenderer.color = new Color(1, 1, 1, 0.3f); // Semi-transparent to indicate invisibility

        yield return new WaitForSeconds(invisibilityDuration);

        // Remove invisibility
        spriteRenderer.color = new Color(1, 1, 1, 1);

        transform.parent.gameObject.tag = "Player";        
        // Destroy the clone after a while (optional, if you want clones to disappear after time)
        Destroy(clone); // Adjust lifespan as needed
        OnPlayerInvisible?.Invoke();
    }
}