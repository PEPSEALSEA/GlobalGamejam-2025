using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; } // Singleton
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthDecreaseRate = 1f;
    public float healthIncreaseAmount = 20f;

    private Animator animator; // Reference to the Animator component
    private PlayerMovement playerMovement; // Reference to PlayerMovement
    private Rigidbody2D rb; // Reference to Rigidbody2D
    private bool isDead = false; // Flag to track death state
    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component is missing on the player!");
        }

        // Get the PlayerMovement component
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogWarning("PlayerMovement component is missing on the player!");
        }

        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody2D component is missing on the player!");
        }
    }

    private void Start()
    {
        // Initialize health values and UI
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

    }

    private void Update()
    {
        // Check if health reaches 0 and handle death
        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void AddHealth(float amount)
    {
        // Increase health and clamp it to the max value
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    public void RemoveHealth(float amount)
    {
        // Decrease health and prevent it from going below 0
        currentHealth -= amount;
        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
        }
        UpdateHealthUI();
    }

    private void DecreaseHealthOverTime()
    {
        // Reduce health periodically
        if (currentHealth > 0)
        {
            RemoveHealth(healthDecreaseRate);
        }
    }

    private void UpdateHealthUI()
    {
        // Animate the health slider using DOTween
        healthSlider.DOValue(currentHealth, 0.5f).SetEase(Ease.InOutSine);
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        // Ensure this is executed only once

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Dead");
            if (isDead) return;
            isDead = true; // Set the death flag
        }

        // Disable PlayerMovement component
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Delete Rigidbody2D component
        if (rb != null)
        {
            Destroy(rb);
            Debug.Log("Rigidbody2D component removed.");
        }

        GameManager.Instance.ToggleScoreAddition(false);
        CloudManager.Instance.cloudSpeed = 0f;
        // Additional death logic can be added here
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bubblegum"))
        {
            AddHealth(healthIncreaseAmount);

            other.transform.DOScale(Vector3.zero, 0.5f)
                .OnComplete(() =>
                {
                    Destroy(other.gameObject);
                });
        }
    }
}
