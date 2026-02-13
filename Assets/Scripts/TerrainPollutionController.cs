using UnityEngine;

[ExecuteAlways]
public class TerrainPollutionController : MonoBehaviour
{
    [Header("References")]
    public Terrain terrain;

    [Header("Layers")]
    public int pollutedLayerIndex = 2; // index of corrupted grass
    public int cleanLayerIndex = 0;    // index of clean grass

    [Header("Active Pollution Range (0–1)")]
    [Tooltip("Below this, terrain is fully clean")]
    [Range(0f, 1f)]
    public float rangeMin = 0.4f;

    [Tooltip("Above this, terrain is fully polluted")]
    [Range(0f, 1f)]
    public float rangeMax = 0.8f;

    private float pollution = 0f;  // 1 = fully polluted, 0 = fully clean
    private float lastEffectivePollution = -1f;
    private float[,,] baseAlphas;  // snapshot of original splatmap

    void Awake()
    {
        if (terrain == null)
            terrain = GetComponent<Terrain>();

        if (terrain == null) return;

        var data = terrain.terrainData;
        baseAlphas = data.GetAlphamaps(0, 0, data.alphamapWidth, data.alphamapHeight);
    }

    void Update()
    {
        if (PollutionController.Instance == null) return;
        if (terrain == null || baseAlphas == null) return;

        pollution = Mathf.Clamp01(PollutionController.Instance.pollution);

        // Map 0–1 pollution into 0–1 effective pollution based on custom range
        float effectivePollution = Mathf.InverseLerp(rangeMin, rangeMax, pollution);
        effectivePollution = Mathf.Clamp01(effectivePollution);

        // Only re-apply if the effective value changed
        if (!Mathf.Approximately(effectivePollution, lastEffectivePollution))
        {
            ApplyPollution(effectivePollution);
        }
    }

    void ApplyPollution(float effectivePollution)
    {
        TerrainData data = terrain.terrainData;
        int w = data.alphamapWidth;
        int h = data.alphamapHeight;
        int layers = data.alphamapLayers;

        if (pollutedLayerIndex < 0 || pollutedLayerIndex >= layers ||
            cleanLayerIndex < 0 || cleanLayerIndex >= layers)
        {
            Debug.LogError("Layer indices out of range.");
            return;
        }

        // Work in a separate array so baseAlphas stays unchanged
        float[,,] alphas = (float[,,])baseAlphas.Clone();

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                float pollutedBase = baseAlphas[y, x, pollutedLayerIndex];
                float cleanBase = baseAlphas[y, x, cleanLayerIndex];

                // total amount of "grass" at this texel
                float totalGrass = pollutedBase + cleanBase;
                if (totalGrass <= 0f)
                    continue; // no grass here, leave other layers as they were

                // effectivePollution:
                // 1 → 100% polluted, 0 → 100% clean (after applying range)
                alphas[y, x, pollutedLayerIndex] = totalGrass * effectivePollution;
                alphas[y, x, cleanLayerIndex]  = totalGrass * (1f - effectivePollution);
            }
        }

        data.SetAlphamaps(0, 0, alphas);
        lastEffectivePollution = effectivePollution;
    }
}
