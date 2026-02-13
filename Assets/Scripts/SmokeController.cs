using UnityEngine;

[ExecuteAlways]
public class SmokeController : MonoBehaviour
{
    [Header("Particle System")]
    public ParticleSystem particleSystem;

    [Header("Active Range")]
    [Tooltip("Below this → fully clean")]
    [Range(0f, 1f)]
    public float rangeMin = 0.4f;

    [Tooltip("Above this → fully polluted")]
    [Range(0f, 1f)]
    public float rangeMax = 0.8f;

    [Header("Colors (Random Between Two)")]
    public Color pollutedColorA = new Color(0.55f, 0.55f, 0.55f);
    public Color pollutedColorB = new Color(0.4f, 0.4f, 0.4f);

    public Color cleanColorA = new Color(0.95f, 0.95f, 0.95f);
    public Color cleanColorB = new Color(0.85f, 0.85f, 0.85f);

    private float pollution = 0f;

    void Reset()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (particleSystem == null) return;
	if (PollutionController.Instance == null) return;
	
	pollution = Mathf.Clamp01(PollutionController.Instance.pollution);

        // Convert pollution (0–1) into effectiveBlend (0–1)
        float t = Mathf.InverseLerp(rangeMin, rangeMax, pollution);
        t = Mathf.Clamp01(t);

        // Lerp both min and max colors
        Color finalA = Color.Lerp(cleanColorA, pollutedColorA, t);
        Color finalB = Color.Lerp(cleanColorB, pollutedColorB, t);

        // Apply as Random Between Two Colors
        var main = particleSystem.main;
        main.startColor = new ParticleSystem.MinMaxGradient(finalA, finalB);
    }
}
