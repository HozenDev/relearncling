using UnityEngine;

[ExecuteAlways]
public class FogController : MonoBehaviour
{
    [Header("Fog Settings")]
    public Color pollutedFogColor = Color.grey;
    public Color cleanFogColor = new Color(0.8f, 0.8f, 0.8f, 0f); // very faint
    public float maxFogDensity = 0.04f;
    public float minFogDensity = 0f;

    [Header("Active Fog Range")]
    [Tooltip("Pollution value at which fog starts appearing")]
    public float rangeMin = 0.4f;

    [Tooltip("Pollution value at which fog is fully visible")]
    public float rangeMax = 0.8f;

    private void OnEnable()
    {
        RenderSettings.fog = true;
    }

    private void Update()
    {
	if (PollutionController.Instance == null)
            return;

        float pollution = PollutionController.Instance.pollution;
	
        ApplyFog(pollution);
    }

    private void ApplyFog(float pollution)
    {
        // Lerp fog color from polluted → clean
	float fogAmount = Mathf.InverseLerp(rangeMin, rangeMax, pollution);
	
        RenderSettings.fogColor = Color.Lerp(cleanFogColor, pollutedFogColor, fogAmount);

        // Lerp fog density from 0 → max
        RenderSettings.fogDensity = Mathf.Lerp(minFogDensity, maxFogDensity, fogAmount);
    }
}
