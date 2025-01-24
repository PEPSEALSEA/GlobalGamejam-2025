using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseJumpForce = 5f;
    public float maxDistance = 3f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;
    private Camera mainCamera;

    private GameObject clone;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && isGrounded)
        {
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 direction = (touchPosition - (Vector2)transform.position).normalized;
            float distance = Mathf.Clamp(Vector2.Distance(touchPosition, transform.position), 0, maxDistance);

            float jumpForce = baseJumpForce * (distance / maxDistance);

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
        }

        HandleMirroring();
    }

    void HandleMirroring()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        bool isOffLeft = viewportPosition.x < 0;
        bool isOffRight = viewportPosition.x > 1;

        if (isOffLeft || isOffRight)
        {
            if (clone == null)
            {
                clone = Instantiate(gameObject, transform.position, transform.rotation);
                Destroy(clone.GetComponent<PlayerMovement>());
            }

            Vector3 clonePosition = transform.position;

            if (isOffLeft)
                clonePosition.x += mainCamera.orthographicSize * 2 * mainCamera.aspect;
            else if (isOffRight)
                clonePosition.x -= mainCamera.orthographicSize * 2 * mainCamera.aspect;

            clone.transform.position = clonePosition;
        }
        else if (clone != null)
        {
            Destroy(clone);
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