using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The target object the camera should follow (Your Player Capsule).")]
    public Transform target; // Assign your Player Capsule to this in the Inspector

    [Tooltip("How far behind and above the target the camera should be.")]
    public Vector3 offset = new Vector3(0f, 5f, -10f); // Default offset - adjust in Inspector! (X, Y, Z)

    [Tooltip("How quickly the camera snaps to the target position. Lower values are slower/smoother.")]
    [Range(0.01f, 1.0f)] // Use a slider in the Inspector for easier tuning
    public float smoothFactor = 0.1f; 

    // LateUpdate is called every frame, but *after* all Update calls have finished.
    // This is the best place for camera logic, as it ensures the target has finished moving for the frame.
    void LateUpdate()
    {
        // Check if the target exists to avoid errors if it's destroyed
        if (target == null)
        {
            Debug.LogWarning("Camera Follow target not set!");
            return; // Stop executing if no target
        }

        // Calculate the desired position for the camera:
        // Target's current position + the desired offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the camera's current position and the desired position.
        // Vector3.Lerp creates a gradual movement instead of an instant snap.
        // The 'smoothFactor' determines how much of the distance is covered each frame (e.g., 0.1 = 10%).
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFactor);

        // Apply the calculated smoothed position to the camera's transform
        transform.position = smoothedPosition;

        // Optional: Make the camera always look at the target
        // Uncomment the line below if you want the camera to rotate to face the player.
        // Keep it commented out if you want the camera to maintain a fixed rotation relative to the world.
        // transform.LookAt(target); 
    }
}
