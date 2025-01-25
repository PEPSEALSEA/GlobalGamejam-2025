using UnityEngine;

public class EnemyWorn : BaseEnemy
{
    private readonly string[] slideAnimations = { "Slide1", "Slide2", "Slide3", "Slide4", "Slide5", "Slide6" };

    [SerializeField] private GameObject redLine;
    [SerializeField] private float lineDisplayDuration = 0.5f;

    private void OnEnable()
    {
        string randomSlideAnim = slideAnimations[Random.Range(0, slideAnimations.Length)];
        animator.Play(randomSlideAnim, 0, 0f);
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
