using UnityEngine;
using UnityEngine.UI; // If you want to use UI elements like Text
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 10f; // Duration of the countdown in seconds
    public GameObject tmpText;       // The GameObject to activate when the countdown ends
    public TMP_Text countdownDisplay;    // UI Text to display the countdown timer

    private float timer;
    private bool isCountingDown = false;

    void Start()
    {
        if (tmpText != null)
        {
            tmpText.SetActive(false); // Ensure the GameObject is initially inactive
        }

        if (countdownDisplay != null)
        {
            countdownDisplay.text = countdownTime.ToString("F1"); // Display initial time
        }

        StartCountdown(); // Start the countdown
    }

    void Update()
    {
        if (isCountingDown)
        {
            timer -= Time.deltaTime; // Decrease the timer by the time elapsed since the last frame

            if (countdownDisplay != null)
            {
                countdownDisplay.text = Mathf.Max(timer, 0).ToString("F1"); // Update the display in real-time
            }

            if (timer <= 0f)
            {
                timer = 0f; // Clamp the timer to 0
                EndCountdown();
            }
        }
    }

    public void StartCountdown()
    {
        timer = countdownTime; // Initialize the timer
        isCountingDown = true; // Start the countdown
    }

    private void EndCountdown()
    {
        isCountingDown = false; // Stop the countdown

        if (tmpText != null)
        {
            tmpText.SetActive(true); // Activate the GameObject
        }
    }
}
