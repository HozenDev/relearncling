using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearWithPollution : MonoBehaviour
{
    [Tooltip("Pollution en dessous de laquelle l'objet commence à apparaître (0 = vert, 1 = pollué)")]
    public float appearBelowPollution = 0.7f;

    [Tooltip("Pollution en dessous de laquelle l'objet est complètement visible")]
    public float fullyVisibleBelowPollution = 0.3f;

    private float deltaApperance = 0.1f;
    
    private Vector3 initialScale;

    private Transform my_transform;

    void Awake()
    {
	my_transform = GetComponent<Transform>();
        initialScale = my_transform.localScale;

	appearBelowPollution = Random.Range(appearBelowPollution, appearBelowPollution + deltaApperance);
        fullyVisibleBelowPollution = Random.Range(fullyVisibleBelowPollution, fullyVisibleBelowPollution + deltaApperance);
    }

    void Update()
    {
        if (PollutionController.Instance == null) return;

        float t = PollutionController.Instance.pollution;

        if (t > appearBelowPollution) // 
        {
            my_transform.localScale = Vector3.zero;
        }
        else if (t <= fullyVisibleBelowPollution)
        {
            my_transform.localScale = initialScale;
        }
        else
        {
            // Transition
            float range = appearBelowPollution - fullyVisibleBelowPollution;
            float factor = (appearBelowPollution - t) / range; // 0 → 1
            my_transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, factor);
        }
    }
}
