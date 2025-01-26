using UnityEngine;

public class KillerObject : MonoBehaviour
{
    public AudioClip killSound; // Assign the sound in the inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Get or add an AudioSource component to the GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the audio clip and ensure it doesn't loop
        audioSource.clip = killSound;
        audioSource.loop = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                PlayerHealth.Instance.RemoveHealth(200f);
                Debug.Log("Player has been killed by " + gameObject.name);

                // Play the kill sound
                if (killSound != null)
                {
                    audioSource.Play();
                }
            }
        }
    }
}
