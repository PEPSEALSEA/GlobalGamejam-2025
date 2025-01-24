using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float attackDamage = 10f;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Playeer"))
        {
            PlayerHealth.Instance.RemoveHealth(attackDamage);
        }
    }
}