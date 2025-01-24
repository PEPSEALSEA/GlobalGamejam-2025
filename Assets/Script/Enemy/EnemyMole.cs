using UnityEngine;

public class EnemyMole : BaseEnemy
{
    private void OnEnable()
    {
        animator.Play("Jump");
    }

    private System.Collections.IEnumerator DeactivateAfterAnimation()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"));

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }

}
