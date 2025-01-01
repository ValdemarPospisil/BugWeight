using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;     
    [SerializeField] private float smoothSpeed = 0.125f; 
    [SerializeField] private Vector3 offset;   
    private Camera cam; 

    private void Start()
    {
        cam = GetComponent<Camera>();
        cam.backgroundColor = new Color32(0x1B, 0x4B, 0x00, 0xFF);
    }
    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}
