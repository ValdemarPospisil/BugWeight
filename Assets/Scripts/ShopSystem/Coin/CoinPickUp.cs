using System.Collections;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public enum PickUpObject { COIN, GEM };
    [SerializeField] private PickUpObject currentObject;
    [SerializeField] private int coinValue;

    private Rigidbody2D itemRb;
    private CircleCollider2D circle;
    [SerializeField] private float dropForce = 5; // Force applied when the coin spawns
    [SerializeField] private float waitTime = 0.5f; // Time before the coin can be picked up
    [SerializeField] private float lifetime = 5f; // Time before the coin disappears

    private bool canBePickedUp = false;

    void Start()
    {
        itemRb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();

        // Disable the collider initially to prevent immediate pickup
        circle.enabled = false;

        // Apply a random force in a top-down direction
        Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        itemRb.AddForce(dropDirection * dropForce, ForceMode2D.Impulse);

        // Enable the collider after a short delay
        StartCoroutine(EnablePickupAfterDelay(waitTime));

        // Destroy the coin after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Stop the coin's movement
        itemRb.linearVelocity = Vector2.zero;
        itemRb.angularVelocity = 0f;

        // Enable the collider so the player can pick it up
        circle.enabled = true;
        canBePickedUp = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canBePickedUp)
        {
            // Pick up the coin
            StartCoroutine(CurrencyCounter.instance.AddCurrency(coinValue));
            Destroy(gameObject);
            // SoundManager.instance.Play("Coin"); // Uncomment if you have a sound manager
        }
    }
}