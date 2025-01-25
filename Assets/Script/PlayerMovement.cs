using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseJumpForce = 5f;
    public float maxDistance = 3f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public int maxJumps = 2; // Max number of jumps (e.g., 2 for double jump)

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Collider2D playerCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // To handle sprite flipping

    private int currentJumps = 0; // Track current jumps
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Reference to the SpriteRenderer
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Reset jumps when grounded
        if (isGrounded && currentJumps > 0)
        {
            currentJumps = 0;
        }

        // Update animator parameters
        animator.SetBool("isGrounded", isGrounded);

        // Jump logic
        if (Input.GetMouseButtonDown(0) && currentJumps < maxJumps)
        {
            currentJumps++; // Increment the jump count

            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

            // Flip the sprite based on the click direction
            if (mousePosition.x > transform.position.x)
            {
                spriteRenderer.flipX = false; // Face right
            }
            else
            {
                spriteRenderer.flipX = true; // Face left
            }

            float distance = Mathf.Clamp(Vector2.Distance(mousePosition, transform.position), 0, maxDistance);
            float jumpForce = baseJumpForce * (distance / maxDistance);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical velocity
            rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);

            // Trigger the jump animation
            animator.SetTrigger("Jump");

            StartCoroutine(DisableCollisionTemporarily());
        }

        HandleMirroring();
    }

    private System.Collections.IEnumerator DisableCollisionTemporarily()
    {
        int playerLayer = gameObject.layer; // Player's layer
        int groundLayerIndex = Mathf.RoundToInt(Mathf.Log(groundLayer.value, 2)); // Convert LayerMask to layer index

        // Disable collisions between player and groundLayer
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayerIndex, true);

        // Wait until the player's upward velocity becomes zero or negative
        while (rb.linearVelocity.y > 0)
        {
            yield return null;
        }

        // Re-enable collisions
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayerIndex, false);
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
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
