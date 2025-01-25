using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpForce = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, jumpForce);
            }
        }
    }
}