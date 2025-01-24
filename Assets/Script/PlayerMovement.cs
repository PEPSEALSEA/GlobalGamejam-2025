using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseJumpForce = 5f; // Base force for movement
    public float maxDistance = 3f; // Maximum distance that affects jump force
    public LayerMask groundLayer; // Layer considered as ground
    public Transform groundCheck; // Ground check position
    public float groundCheckRadius = 0.2f; // Radius for ground check

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Collider2D playerCollider;
    private GameObject clone;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        playerCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Check for mouse click and grounded state
        if (Input.GetMouseButtonDown(0) && isGrounded)
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // Calculate direction from player to mouse position
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

            // Clamp the distance for consistent force
            float distance = Mathf.Clamp(Vector2.Distance(mousePosition, transform.position), 0, maxDistance);

            // Calculate force based on distance
            float jumpForce = baseJumpForce * (distance / maxDistance);

            // Reset velocity and apply force toward the mouse position
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);

            StartCoroutine(DisableCollisionTemporarily());
        }

        HandleMirroring();
    }

    private System.Collections.IEnumerator DisableCollisionTemporarily()
    {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMaskToLayer(groundLayer), true);

        while (rb.linearVelocity.y > 0)
        {
            yield return null;
        }

        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMaskToLayer(groundLayer), false);
    }

    private int LayerMaskToLayer(LayerMask layerMask)
    {
        int layer = 0;
        int bitmask = layerMask.value;
        while (bitmask > 1)
        {
            bitmask >>= 1;
            layer++;
        }
        return layer;
    }

    void HandleMirroring()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        bool isOffLeft = viewportPosition.x < 0;
        bool isOffRight = viewportPosition.x > 1;

        if (isOffLeft || isOffRight)
        {
            Vector3 position = transform.position;

            if (isOffLeft)
                position.x += mainCamera.orthographicSize * 2 * mainCamera.aspect;
            else if (isOffRight)
                position.x -= mainCamera.orthographicSize * 2 * mainCamera.aspect;

            transform.position = position;
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
