using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // Reference to the player's transform
    public float smoothSpeed = 0.125f;  // Smooth speed for camera movement
    public Vector3 offset;           // Offset from the player's position

    void LateUpdate()
    {
        if (player != null)
        {
            // Calculate the desired position with offset
            Vector3 desiredPosition = player.position + offset;

            // Smoothly interpolate between the camera's current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;
        }
    }
}
