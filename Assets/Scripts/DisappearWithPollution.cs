using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearWithLowPollution : MonoBehaviour
{
    [Tooltip("Pollution au-dessus de laquelle l'objet commence à apparaître (0 = vert, 1 = pollué)")]
    public float appearAbovePollution = 0.3f;

    [Tooltip("Pollution au-dessus de laquelle l'objet est complètement visible")]
    public float fullyVisibleAbovePollution = 0.7f;

    private float deltaApperance = 0.1f;

    private Vector3 initialScale;
    private Transform my_transform;

    void Awake()
    {
        my_transform = GetComponent<Transform>();
        initialScale = my_transform.localScale;

        // Randomise thresholds as in your original script
        appearAbovePollution = Random.Range(appearAbovePollution, appearAbovePollution + deltaApperance);
        fullyVisibleAbovePollution = Random.Range(fullyVisibleAbovePollution, fullyVisibleAbovePollution + deltaApperance);
    }

    void Update()
    {
        if (PollutionController.Instance == null) return;

        float t = PollutionController.Instance.pollution;

        if (t < appearAbovePollution)
        {
            // Pollution too low → object fully hidden
            my_transform.localScale = Vector3.zero;
        }
        else if (t >= fullyVisibleAbovePollution)
        {
            // Pollution high enough → object fully visible
            my_transform.localScale = initialScale;
        }
        else
        {
            // Transition between hidden → visible
            float range = fullyVisibleAbovePollution - appearAbovePollution;
            float factor = (t - appearAbovePollution) / range; // 0 → 1
            my_transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, factor);
        }
    }
}
