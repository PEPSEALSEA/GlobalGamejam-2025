using UnityEngine;

public class EnemyMole : BaseEnemy
{
    private readonly string[] jumpAnimations = { "Jump1", "Jump2", "Jump3", "Jump4", "Jump5", "Jump6", "Jump7", "Jump8", "Jump9", "Jump10", "Jump11", "Jump12" };

    [SerializeField] private GameObject redLine;
    [SerializeField] private float lineDisplayDuration = 0.5f;

    private void OnEnable()
    {
        string randomJumpAnim = jumpAnimations[Random.Range(0, jumpAnimations.Length)];
        animator.Play(randomJumpAnim, 0, 0f);
        animator.speed = 0;

        if (redLine != null)
        {
            redLine.SetActive(true);
        }

        StartCoroutine(ResumeAnimationAfterDelay());
    }

    private System.Collections.IEnumerator ResumeAnimationAfterDelay()
    {
        yield return new WaitForSeconds(lineDisplayDuration);

        // Hide the red line
        if (redLine != null)
        {
            redLine.SetActive(false);
        }

        animator.speed = 1;

        StartCoroutine(DeactivateAfterAnimation());
    }

    private System.Collections.IEnumerator DeactivateAfterAnimation()
    {
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
