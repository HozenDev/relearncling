using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RiverLevelController : MonoBehaviour
{
    [Header("River heights")]
    public float dryHeight = -1f;
    public float fullHeight = 0.5f;

    [Header("Pollution range (0 = clean, 1 = polluted)")]
    public float pollutionStart = 1f;
    public float pollutionEnd = 0.4f;

    [Header("Logarithmic curve")]
    [Tooltip("1 = linear, higher = more logarithmic (slow start, fast end)")]
    public float logExponent = 3f;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        if (PollutionController.Instance == null)
            return;

        float pollution = PollutionController.Instance.pollution;

        // Map pollution into 0..1 inside the chosen range
        float t = Mathf.InverseLerp(pollutionStart, pollutionEnd, pollution);
        t = Mathf.Clamp01(t);

        float logT = Mathf.Pow(t, 1f / logExponent);

        float y = Mathf.Lerp(dryHeight, fullHeight, logT);

        Vector3 pos = rb.position;
        pos.y = y;
        rb.MovePosition(pos);
    }
}
