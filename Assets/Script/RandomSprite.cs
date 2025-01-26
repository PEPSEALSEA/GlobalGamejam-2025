using UnityEngine;

public class RandomSpriteOnEnable : MonoBehaviour
{
    // List of sprites to choose from
    public Sprite[] sprites;

    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Check if the SpriteRenderer exists
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject.");
        }
    }

    private void OnEnable()
    {
        // Ensure there are sprites in the list
        if (sprites != null && sprites.Length > 0)
        {
            // Choose a random sprite from the list
            int randomIndex = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[randomIndex];
        }
        else
        {
            Debug.LogWarning("No sprites assigned in the list.");
        }
    }
}

