using UnityEngine;

// Ensure this GameObject has a Rigidbody component
[RequireComponent(typeof(Rigidbody))] 
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How fast the character moves.")]
    public float moveSpeed = 5f; // You can adjust this speed in the Inspector

    private Rigidbody rb; // To store the Rigidbody component
    private Vector3 moveInput; // To store the calculated input direction

    // Awake is called when the script instance is being loaded (before Start)
    void Awake()
    {
        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();

        // Optional: Add some constraints to the Rigidbody in code 
        // (You can also do this in the Inspector)
        // Prevents the capsule from tipping over
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; 
    }

    // Update is called once per frame
    void Update()
    {
        // --- Input Handling ---
        // Get input values from horizontal (A/D, Left/Right Arrows, Controller Left Stick X)
        float horizontalInput = Input.GetAxis("Horizontal"); 
        // Get input values from vertical (W/S, Up/Down Arrows, Controller Left Stick Y)
        float verticalInput = Input.GetAxis("Vertical");   

        // Create a direction vector based on input. 
        // We use transform.right and transform.forward to ensure movement is relative
        // to the character's orientation IF you wanted that. For world-relative movement:
        // moveInput = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        
        // For movement relative to world directions (usually preferred for simple top-down or 3rd person):
        moveInput = new Vector3(horizontalInput, 0f, verticalInput);

        // Normalize the vector if moving diagonally shouldn't be faster
        if (moveInput.magnitude > 1f) 
        {
            moveInput.Normalize();
        }
    }

    // FixedUpdate is called at a fixed interval, ideal for physics calculations
    void FixedUpdate()
    {
        // --- Movement Logic ---
        // Calculate the desired velocity
        Vector3 targetVelocity = moveInput * moveSpeed;

        // Keep the existing vertical velocity (for gravity, jumping later)
        targetVelocity.y = rb.linearVelocity.y; 

        // Apply the velocity to the Rigidbody
        rb.linearVelocity = targetVelocity;
    }
}