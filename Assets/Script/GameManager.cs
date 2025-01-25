using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TMP_Text scoreText;
    public float timeInterval = 0.001f;
    public int scorePerInterval = 1;

    public event Action<int> OnScoreUpdated;
    public int currentScore;

    private int displayedScore = 0;
    private int lastPopupScore = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

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

            if (currentScore >= lastPopupScore + 100)
            {
                lastPopupScore = currentScore;
                TriggerScorePopup();
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
                int rollingDigit = (currentDigit + 5) % 10;
                animatedText += rollingDigit.ToString();
            }
            else
            {
                animatedText += targetDigit.ToString();
            }
        }

        scoreText.text = $"Score: {animatedText}";
    }

    void TriggerScorePopup()
    {
        Vector3 originalScale = scoreText.transform.localScale;

        scoreText.transform.DOScale(originalScale * 1.5f, 0.2f)
            .OnComplete(() =>
            {
                scoreText.transform.DOScale(originalScale, 0.2f);
            });

        Color originalColor = scoreText.color;
        scoreText.DOColor(Color.yellow, 0.2f)
            .OnComplete(() =>
            {
                scoreText.DOColor(originalColor, 0.2f);
            });
    }

    public void AddHealth(int amount)
    {
        Debug.Log($"Health increased by {amount}!");
    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (currentScore >= lastPopupScore + 100)
        {
            lastPopupScore = currentScore;
            TriggerScorePopup();
        }
    }
}
