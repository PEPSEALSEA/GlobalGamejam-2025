using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteIntensityController : MonoBehaviour
{
    public Volume globalVolume; // Assign your Global Volume here
    private Vignette vignette;

    void Start()
    {
        // Try to get the Vignette effect from the volume profile
        if (globalVolume != null && globalVolume.profile.TryGet(out vignette))
        {
            // Successfully initialized vignette reference
        }
        else
        {
            Debug.LogError("Vignette effect not found in the Global Volume profile.");
        }
    }

    public void DecreaseVignetteIntensity()
    {
        if (vignette != null)
        {
            StartCoroutine(DecreaseIntensityOverTime(1f)); // Decrease over 1 second
        }
    }

    private System.Collections.IEnumerator DecreaseIntensityOverTime(float duration)
    {
        float startIntensity = vignette.intensity.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.Override(Mathf.Lerp(startIntensity, 0f, elapsedTime / duration));
            yield return null;
        }

        vignette.intensity.Override(0f); // Ensure it ends exactly at 0
    }
}
