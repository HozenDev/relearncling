using UnityEngine;
using UnityEngine.Rendering; // for AmbientMode

public class SkyboxPollutionBlender : MonoBehaviour
{
    [Tooltip("Material using Custom/Blended Cubemap Skybox")]
    public Material skyboxMaterial;

    [Header("Pollution range (0 = clean, 1 = polluted)")]
    public float pollutionStart = 1f; // start blending here
    public float pollutionEnd   = 0.4f; // fully polluted here

    [Header("Environment lighting")]
    [Tooltip("If true, call DynamicGI.UpdateEnvironment() when blend changes")]
    public bool updateEnvironmentGI = true;

    [Tooltip("Minimum change in blend before we update GI (to avoid doing it every frame)")]
    public float giUpdateThreshold = 0.02f;

    private float _lastBlend = -1f;

    void OnEnable()
    {
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;

            // Make sure ambient comes from the skybox
            RenderSettings.ambientMode = AmbientMode.Skybox;
        }
    }

    void Update()
    {
        if (skyboxMaterial == null || PollutionController.Instance == null)
            return;

        float pollution = Mathf.Clamp01(PollutionController.Instance.pollution);

        // Map pollution into 0..1 over the chosen window
        float t = Mathf.Lerp(pollutionEnd, pollutionStart, pollution);
        t = Mathf.Clamp01(t);

        skyboxMaterial.SetFloat("_Blend", t);

        // Only update GI if we actually changed enough
        if (updateEnvironmentGI && Mathf.Abs(t - _lastBlend) > giUpdateThreshold)
        {
            _lastBlend = t;
            DynamicGI.UpdateEnvironment(); // refresh ambient + reflection from skybox
        }
    }
}
