using System.Collections;
using UnityEngine;

public class BloodAttraction : MonoBehaviour
{
    private Transform player;
    private ParticleSystem ps;
    private float delay = 0.7f; // Time before particles start moving
    private float moveSpeed = 1f; // Speed of movement

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(StartAttraction());
    }

    IEnumerator StartAttraction()
    {
        yield return new WaitForSeconds(delay);
        while (ps != null && player != null)
        {
            // Move the particles towards the player
            transform.position = Vector3.Lerp(transform.position, player.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
