using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "SpecialAbilities/Decoy")]
public class DecoyAbility : SpecialAbility
{
    [SerializeField] private GameObject decoyPrefab;
    [SerializeField] private float invisibilityDuration = 5f;

    public override void Activate()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var decoy = Instantiate(decoyPrefab, player.transform.position, Quaternion.identity);

        player.GetComponent<Renderer>().enabled = false;
        player.GetComponent<Collider2D>().enabled = false;

        // Re-enable player visibility after duration
        player.GetComponent<MonoBehaviour>().StartCoroutine(ReactivatePlayer(player));
    }

    private IEnumerator ReactivatePlayer(GameObject player)
    {
        yield return new WaitForSeconds(invisibilityDuration);
        player.GetComponent<Renderer>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
    }
}
