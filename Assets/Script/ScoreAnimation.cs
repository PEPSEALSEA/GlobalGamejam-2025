using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class ScoreAnimation : MonoBehaviour
{
    public TMP_Text scoreText; // Reference to the UI Text component
    public float animationDuration = 1.5f;
    public string prefname = "";

    void OnEnable()
    {
        // Get the last score from PlayerPrefs
        int lastScore = PlayerPrefs.GetInt(prefname, 0);

        // Call the function to animate the score
        AnimateScore(lastScore);
    }

    void AnimateScore(int targetScore)
    {
        int displayedScore = 0; // Starting value for the animation
        DOTween.To(() => displayedScore, x => 
        {
            displayedScore = x;
            scoreText.text = displayedScore.ToString(); // Update the UI text
        }, targetScore, animationDuration)
        .SetEase(Ease.OutCubic); // Smooth easing for the animation
    }
}
