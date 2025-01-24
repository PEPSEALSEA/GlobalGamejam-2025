using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public float timeInterval = 0.001f;
    public int scorePerInterval = 1;

    private int currentScore = 0;
    private int displayedScore = 0;

    void Start()
    {
        StartCoroutine(AddScoreOverTime());
    }

    IEnumerator AddScoreOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeInterval);
            currentScore += scorePerInterval;

            if (!DOTween.IsTweening("ScoreAnimation"))
            {
                DOTween.To(() => displayedScore, x =>
                {
                    displayedScore = x;
                    UpdateScoreUIWithSlotEffect(x);
                }, currentScore, 0.5f).SetId("ScoreAnimation");
            }
        }
    }

    void UpdateScoreUIWithSlotEffect(int targetScore)
    {
        string targetScoreString = targetScore.ToString();
        string displayedScoreString = displayedScore.ToString();

        int maxLength = Mathf.Max(targetScoreString.Length, displayedScoreString.Length);
        targetScoreString = targetScoreString.PadLeft(maxLength, '0');
        displayedScoreString = displayedScoreString.PadLeft(maxLength, '0');

        string animatedText = "";

        for (int i = 0; i < maxLength; i++)
        {
            int currentDigit = displayedScoreString[i] - '0';
            int targetDigit = targetScoreString[i] - '0';

            if (currentDigit != targetDigit)
            {
                int rollingDigit = (currentDigit + Random.Range(1, 10)) % 10;
                animatedText += rollingDigit.ToString();
            }
            else
            {
                animatedText += targetDigit.ToString();
            }
        }

        scoreText.text = $"Score: {animatedText}";
    }

    void UpdateScoreUI()
    {
        scoreText.text = $"Score: {displayedScore}";
    }
}
