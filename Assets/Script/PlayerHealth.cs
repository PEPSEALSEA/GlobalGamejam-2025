using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Playables; // Required for PlayableDirector
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deeadscene;
    public static PlayerHealth Instance { get; private set; } // Singleton
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthDecreaseRate = 1f;
    public float healthIncreaseAmount = 20f;
    public PlayableDirector timelineDirector; // Public variable for timeline


    private Animator animator; // Reference to the Animator component
    private PlayerMovement playerMovement; // Reference to PlayerMovement
    private Rigidbody2D rb; // Reference to Rigidbody2D
    private bool isDead = false; // Flag to track death state

    [Header("BoostSpeed")]
    public float defaultSpeed = 2f;  // Default cloud speed
    public float boostedSpeed = 5f; // Boosted speed value
    public float toggleDuration = 5f; // Duration for the speed effect

    private Transform targetObject; // The object this bubble sticks to
    private Rigidbody2D playerRigidbody; // Player's Rigidbody2D
    private Collider2D playerCollider;
    private bool originalGravityState;

    [Header("Audio")]
    public AudioSource intoBubble;
    public AudioSource windBubble;

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
        if (isDead) return;
        isDead = true;

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("Dead");
            animator.SetBool("isDead", true);
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

        // Stop adding scores
        GameManager.Instance.ToggleScoreAddition(false);

        // Stop cloud movement
        CloudManager.Instance.cloudSpeed = 0f;

        // Rotate and move after a 2-second delay
        Transform playerTransform = transform;
        Vector3 targetRotation = new Vector3(0f, 0f, 90f);
        Vector3 targetPosition = playerTransform.position + new Vector3(0f, -15f, 0f);

        DOVirtual.DelayedCall(2f, () =>
        {
            // Move down and rotate over 5 seconds
            playerTransform
                .DOMove(targetPosition, 5f)
                .SetEase(Ease.InOutQuad);

            playerTransform
                .DORotate(targetRotation, 5f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    deeadscene.SetActive(true);
                    // // Play the timeline
                    // if (timelineDirector != null)
                    // {
                    //     timelineDirector.Play();
                    //     Debug.Log("Timeline is now playing.");

                    // }
                    // else
                    // {
                    //     Debug.LogWarning("PlayableDirector is not assigned!");
                    // }
                });
        });
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

        if (other.gameObject.CompareTag("SpeedBubble"))
        {
            targetObject = other.transform;
            playerRigidbody = GetComponent<Rigidbody2D>();
            playerCollider = GetComponent<Collider2D>();

            if (playerRigidbody != null)
            {
                StartCoroutine(ApplySpeedBoost());
            }
        }
    }

    public IEnumerator ApplySpeedBoost()
    {
        originalGravityState = playerRigidbody.gravityScale > 0;
        playerRigidbody.gravityScale = 0;
        playerRigidbody.linearVelocity = Vector2.zero;
        playerRigidbody.isKinematic = true;
        playerCollider.enabled = false;

        EnemyManager.Instance.StopSpawner();

        transform.SetParent(targetObject);
        transform.localPosition = Vector3.zero;

        float originalSpeed = CloudManager.Instance.cloudSpeed;
        float boostedSpeed = originalSpeed * 5f; // 5x the current speed

        DOTween.To(() => CloudManager.Instance.cloudSpeed,
               x => CloudManager.Instance.cloudSpeed = x,
               boostedSpeed,
               1f);

        GameManager.Instance.ToggleBonusTime(true);

        yield return new WaitForSeconds(toggleDuration);

        DOTween.To(() => CloudManager.Instance.cloudSpeed,
               x => CloudManager.Instance.cloudSpeed = x,
               originalSpeed,
               1f);

        GameManager.Instance.ToggleBonusTime(false);

        EnemyManager.Instance.StartSpawner();

        CloudManager.Instance.cloudSpeed = 2f;

        playerRigidbody.isKinematic = false;
        playerCollider.enabled = true;
        playerRigidbody.gravityScale = 2f;

        transform.SetParent(null);

        targetObject.gameObject.SetActive(false);

        Destroy(targetObject.gameObject);
    }

    private IEnumerator playStartSound()
    {
        intoBubble.Play();
        yield return new WaitForSeconds(1f);
        windBubble.Play();
    }
}
