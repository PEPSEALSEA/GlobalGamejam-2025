using UnityEngine;
using UnityEngine.UI;

public class RandomImageOnEnable : MonoBehaviour
{
    // List of sprites to choose from
    public Sprite[] sprites;

    // Reference to the Image component
    private Image imageComponent;
    public GameObject disabledd;

    private void Awake()
    {
        disabledd.SetActive(false);
        // Get the Image component
        imageComponent = GetComponent<Image>();

        // Check if the Image component exists
        if (imageComponent == null)
        {
            Debug.LogError("No Image component found on this GameObject.");
        }
    }

    private void OnEnable()
    {
        // Ensure there are sprites in the list
        if (sprites != null && sprites.Length > 0)
        {
            // Choose a random sprite from the list
            int randomIndex = Random.Range(0, sprites.Length);
            imageComponent.sprite = sprites[randomIndex];
        }
        else
        {
            Debug.LogWarning("No sprites assigned in the list.");
        }
    }
}
