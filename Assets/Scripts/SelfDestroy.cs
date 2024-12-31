using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 2f;
    
    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}
