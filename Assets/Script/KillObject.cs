using UnityEngine;

public class KillObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is the player
        if (other.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Set player's health to 0 and trigger death
                playerHealth.currentHealth = 0;
                playerHealth.Die();

                Debug.Log("Player has been killed by " + gameObject.name);
            }
        }
    }
}
