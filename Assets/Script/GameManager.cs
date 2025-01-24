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
                    UpdateScoreUI();
                }, currentScore, 0.5f).SetId("ScoreAnimation");
            }
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = $"Score: {displayedScore}";
    }
}
