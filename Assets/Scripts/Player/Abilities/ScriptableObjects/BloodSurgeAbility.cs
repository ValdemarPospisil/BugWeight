using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood Surge", menuName = "SpecialAbilities/Blood Surge")]
public class BloodSurgeAbility : SpecialAbility
{
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    //[SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private float damageDuration = 3f;
    [SerializeField] private GameObject bloodTrailPrefab;

    public override void Activate()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<MonoBehaviour>().StartCoroutine(BloodSurge(player));
        }
    }

    private IEnumerator BloodSurge(GameObject player)
    {
        var playerController = player.GetComponent<PlayerController>();
        if (playerController == null) yield break;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb == null) yield break;

        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (direction == Vector2.zero) yield break;
        var collider = player.GetComponent<Collider2D>();
        
        collider.isTrigger = true;
        
        playerController.IsDashing = true;
        rb.linearVelocity = direction * dashSpeed;

        var bloodTrail = Instantiate(bloodTrailPrefab, player.transform.position, Quaternion.identity);
        bloodTrail.transform.SetParent(player.transform);

        var trailRenderer = player.GetComponent<TrailRenderer>();
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector2.zero;
        bloodTrail.GetComponent<ParticleSystem>().Stop();
        trailRenderer.emitting = false;
        bloodTrail.transform.SetParent(null);
        collider.isTrigger = false;
        playerController.IsDashing = false;

        Destroy(bloodTrail, damageDuration);
    }
}