using UnityEngine;

public class KillerObject : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                PlayerHealth.Instance.RemoveHealth(200f);
                Debug.Log("Player has been killed by " + gameObject.name);

                audioSource.Play();
            }
        }
    }
}
