using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Correct for URP
using DG.Tweening; // Import DOTween namespace

public class FadeControllder : MonoBehaviour
{
    public Volume volume; // Reference to the Volume component for URP
    public GameObject object1; // First object to toggle
    public GameObject object2; // Second object to toggle
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
                // Toggle objects when the fade completes
                if (fadeIn)
                {
                    object1.SetActive(false);
                    object2.SetActive(true);
                }
                else
                {
                    object1.SetActive(true);
                    object2.SetActive(false);
                }
            });
    }
}
