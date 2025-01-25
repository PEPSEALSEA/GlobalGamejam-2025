using UnityEngine;
using System.Collections;

public class SpeedBubble : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D colliderToDisable;

    private void OnEnable()
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned!");
            return;
        }

        if (colliderToDisable != null)
        {
            colliderToDisable.enabled = false;
        }

        animator.Play("startBubble");
        StartCoroutine(DeactivateAfterAnimation());
    }

    private IEnumerator DeactivateAfterAnimation()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
        {
            yield return null; // Wait until the animation finishes
        }

        if (colliderToDisable != null)
        {
            colliderToDisable.enabled = true; // Reactivate the collider if needed
        }
        
    }
}