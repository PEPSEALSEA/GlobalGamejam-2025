using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseJumpForce = 5f; // Base jump force
    public float maxDistance = 3f;  // Maximum distance to calculate jump force
    public LayerMask groundLayer;   // Layer mask for ground detection
    public Transform groundCheck;  // Transform used to check if player is grounded
    public float groundCheckRadius = 0.2f; // Radius of the ground check circle

    private Rigidbody2D rb;
    private Camera mainCamera;

    private GameObject clone; // The mirrored clone of the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the player is grounded
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle touch input
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && isGrounded)
        {
            // Get touch position
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);

            // Calculate direction and distance
            Vector2 direction = (touchPosition - (Vector2)transform.position).normalized;
            float distance = Mathf.Clamp(Vector2.Distance(touchPosition, transform.position), 0, maxDistance);

            // Scale jump force based on distance
            float jumpForce = baseJumpForce * (distance / maxDistance);

            // Apply jump force
            rb.linearVelocity = Vector2.zero; // Reset velocity
            rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
        }

        // Handle mirroring
        HandleMirroring();
    }

    void HandleMirroring()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the player is partially off-screen
        bool isOffLeft = viewportPosition.x < 0;
        bool isOffRight = viewportPosition.x > 1;

        // Create a mirrored clone on the opposite side if necessary
        if (isOffLeft || isOffRight)
        {
            if (clone == null)
            {
                clone = Instantiate(gameObject, transform.position, transform.rotation);
                Destroy(clone.GetComponent<PlayerMovement>()); // Prevent the clone from duplicating itself
            }

            Vector3 clonePosition = transform.position;

            if (isOffLeft)
                clonePosition.x += mainCamera.orthographicSize * 2 * mainCamera.aspect;
            else if (isOffRight)
                clonePosition.x -= mainCamera.orthographicSize * 2 * mainCamera.aspect;

            clone.transform.position = clonePosition;
        }
        else if (clone != null) // Destroy the clone when the player is fully back on screen
        {
            Destroy(clone);
        }
    }

    void OnDrawGizmos()
    {
        // Visualize the ground check radius in the editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
