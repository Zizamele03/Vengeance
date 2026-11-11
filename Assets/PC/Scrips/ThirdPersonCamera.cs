/*using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;       // Reference to your player
    public Vector3 offset = new Vector3(0f, 3f, -6f); // Camera behind + above player
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (player == null) return;

        // Position behind player
        Vector3 desiredPosition = player.position + player.TransformDirection(offset);

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Look at the player
        transform.LookAt(player.position + Vector3.up * 2.0f); // Look at upper body
    }
}
*/