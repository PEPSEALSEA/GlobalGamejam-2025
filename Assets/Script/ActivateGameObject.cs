using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class ActivateGameObjects : MonoBehaviour
{
    // Reference to the Button
    public Button activateButton;

    // Reference to the PlayableDirector (Timeline)
    public PlayableDirector timeline;

    void Start()
    {
        // Ensure the button has a listener
        if (activateButton != null)
        {
            activateButton.onClick.AddListener(PlayTimeline);
        }
    }

    // Method to play the timeline
    private void PlayTimeline()
    {
        if (timeline != null)
        {
            timeline.Play();
        }
    }
} 