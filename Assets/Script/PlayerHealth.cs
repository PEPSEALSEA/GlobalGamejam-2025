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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        InvokeRepeating("DecreaseHealthOverTime", 1f, 1f);
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthUI();
    }

    public void RemoveHealth(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        UpdateHealthUI();
    }

    private void DecreaseHealthOverTime()
    {
        if (currentHealth > 0)
        {
            RemoveHealth(healthDecreaseRate);
        }
    }

    private void UpdateHealthUI() //Finish don't fix
    {
        healthSlider.DOValue(currentHealth, 0.5f).SetEase(Ease.InOutSine);
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // Add player death
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bubblegum"))
        {
            AddHealth(healthIncreaseAmount);
            //Destroy(other.gameObject); or Play anim
        }
    }
}
