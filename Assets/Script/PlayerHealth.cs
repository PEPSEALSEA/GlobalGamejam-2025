using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthSystem : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthDecreaseRate = 1f;
    public float healthIncreaseAmount = 20f;

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
        }
    }

    void DecreaseHealthOverTime()
    {
        if (currentHealth > 0)
        {
            currentHealth -= healthDecreaseRate;
            UpdateHealthUI();
        }
    }
    public void PickupHealthItem()
    {
        currentHealth += healthIncreaseAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }
    void UpdateHealthUI()
    {
        healthSlider.DOValue(currentHealth, 0.5f).SetEase(Ease.InOutSine);
    }
}
