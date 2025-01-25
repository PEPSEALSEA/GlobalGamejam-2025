using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseJumpForce = 5f;
    public float maxDistance = 3f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public float slowdownFactor = 0.5f; // Factor to reduce upward motion when finger is higher

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Collider2D playerCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);

        // Update animator parameters
        animator.SetBool("isGrounded", isGrounded);

        // Handle touch input for mobile-friendly control
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (touchPosition - (Vector2)transform.position).normalized;

            // Flip the sprite based on the direction
            if (touchPosition.x > transform.position.x)
            {
                spriteRenderer.flipX = false; // Face right
            }
            else
            {
                spriteRenderer.flipX = true; // Face left
            }

            float distance = Mathf.Clamp(Vector2.Distance(touchPosition, transform.position), 0, maxDistance);
            float jumpForce = baseJumpForce * (distance / maxDistance);

            // Adjust upward force based on the vertical distance
            if (touchPosition.y > transform.position.y)
            {
                jumpForce *= slowdownFactor; // Slow down upward motion
            }

            if (isGrounded)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);

                // Trigger the jump animation
                animator.SetTrigger("Jump");

                StartCoroutine(DisableCollisionTemporarily());
            }
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
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}
