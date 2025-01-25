using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening; // Import DOTween namespace

public class VignetteIntensityController : MonoBehaviour
{
    public Volume globalVolume; // Assign your Global Volume here
    private Vignette vignette;

    public static VignetteIntensityController Instance { get; private set; }

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


    public void IncressVignetteIntensity()
    {
        if (vignette != null)
        {
            float startIntensity = vignette.intensity.value;

            DOTween.To(
                () => vignette.intensity.value,  // Getter for the current intensity
                x => vignette.intensity.Override(x),  // Setter for the intensity
                0.445f,  // Target value
                1f   // Duration (1 second)
            ).SetEase(Ease.Linear); // Optional: Set an easing function
        }
    }

    public void DecreaseVignetteIntensity()
    {
        if (vignette != null)
        {
            float startIntensity = vignette.intensity.value;

            DOTween.To(
                () => vignette.intensity.value,  // Getter for the current intensity
                x => vignette.intensity.Override(x),  // Setter for the intensity
                0f,  // Target value
                1f   // Duration (1 second)
            ).SetEase(Ease.Linear); // Optional: Set an easing function
        }
    }
}