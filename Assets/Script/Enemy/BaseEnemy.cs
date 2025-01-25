using NUnit.Framework.Interfaces;
using UnityEditor.Overlays;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public Animator animator;
    public float attackDamage;

    protected virtual void Awake()
    {
        if (animator == null)
        {
            if (!TryGetComponent<Animator>(out animator))
            {
                Debug.LogWarning("Animator component not found on this GameObject.");
            }
        }
    }

    protected virtual void Start()
    {
        if (attackDamage == 0f)
        {
            attackDamage = 10f;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(this.gameObject.name + ": Hit You");
            PlayerHealth.Instance.RemoveHealth(attackDamage);
            GameManager.Instance.RemoveScore(50);
        }
    }
}