using UnityEngine;
using System.Collections;
public class test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private Vector2 direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = new Vector2(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
    {
       //rb.linearVelocity = direction * 20f; // Dash right
        //Debug.Log("Testing Dash Velocity: " + rb.linearVelocity);
        StartCoroutine(Dash());
    }
    }

    private IEnumerator Dash()
{
    Debug.Log("Dash started...");

   

    if (direction == Vector2.zero)
    {
        Debug.LogWarning("Cannot dash without a valid direction!");
        yield break;
    }

    Debug.Log("Dash Direction: " + direction);

    float elapsedTime = 0f;

    while (elapsedTime < 0.2f)
    {
        rb.linearVelocity = direction * 20f;
        Debug.Log("Dashing... Velocity: " + rb.linearVelocity);

        elapsedTime += Time.deltaTime;
        yield return null;
    }

    rb.linearVelocity = Vector2.zero; 
    Debug.Log("Dash ended. Velocity reset.");

    yield return new WaitForSeconds(1f);
    Debug.Log("Dash cooldown ended. Ready to dash again.");
}

}
