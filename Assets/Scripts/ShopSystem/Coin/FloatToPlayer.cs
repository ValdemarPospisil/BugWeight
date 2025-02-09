using UnityEngine;

public class FloatToPlayer : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private float acceleration = 1f;
    private float waitTime = 0.5f;

    private bool isFloating = false;

    void Start()
    {
        player = PlayerManager.Instance.gameObject;
    }

    void Update()
    {
        waitTime -= Time.deltaTime;
        if (player != null && waitTime <= 0)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRadius)
            {
                isFloating = true;
            }
            
            if (isFloating)
            {
                speed += acceleration;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}