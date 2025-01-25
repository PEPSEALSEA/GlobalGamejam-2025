using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Correct for URP
using DG.Tweening; // Import DOTween namespace
using UnityEngine.SceneManagement; // Import SceneManager for scene transitions

public class FadeChangescene: MonoBehaviour
{
    public Volume volume; // Reference to the Volume component for URP
    public string nextSceneName; // Name of the scene to transition to
    public float fadeDuration = 1f; // Duration of the fade effect

    private Vignette vignette; // Vignette effect reference

    void Start()
    {
        // Try to get the Vignette effect from the Volume Profile
        if (volume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f; // Start with no vignette effect
        }
        else
        {
            Debug.LogError("No Vignette effect found in the Volume Profile!");
        }
    }

    public void StartFade(bool fadeIn)
    {
        if (vignette == null) return; // Ensure vignette is not null

        float targetValue = fadeIn ? 1f : 0f; // Target intensity for vignette

        // Use DOTween to smoothly tween the vignette intensity
        DOTween.To(() => vignette.intensity.value,
                   x => vignette.intensity.value = x,
                   targetValue,
                   fadeDuration)
            .OnComplete(() =>
            {
                if (fadeIn)
                {
                    // After fade-in, load the next scene
                    ChangeScene();
                }
            });
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); // Load the specified scene
        }
        else
        {
            Debug.LogError("Next scene name is not set!");
        }
    }

    public void StartSceneTransition()
    {
        Debug.Log("Starting scene transition...");
        StartFade(true); // Start fading in
    }
}
