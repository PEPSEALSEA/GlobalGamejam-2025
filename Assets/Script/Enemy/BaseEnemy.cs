using NUnit.Framework.Interfaces;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public Animator animator;
    public float attackDamage = 10f;

    protected virtual void Start()
    {
        if (animator == null)
        {
            if (!TryGetComponent<Animator>(out animator))
            {
                Debug.LogWarning("Animator component not found on this GameObject.");
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Playeer"))
        {
            PlayerHealth.Instance.RemoveHealth(attackDamage);
        }
    }
}