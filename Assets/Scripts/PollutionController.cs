using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionController : MonoBehaviour
{
    public static PollutionController Instance;

    [Range(0f, 1f)]
    public float pollution = 1f;
    // 1 = pollued world
    // 0 = clean world

    public float changeSpeed = 0.2f; // speed changement
    private float targetPollution = 1f;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        pollution = Mathf.MoveTowards(pollution, targetPollution, changeSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Appelle cette fonction depuis ton gameplay (recyclage, score, etc.)
    /// value doit être entre 0 (vert) et 1 (pollué)
    /// </summary>
    public void SetTargetPollution(float value)
    {
        targetPollution = Mathf.Clamp01(value);
    }

    /// <summary>
    /// Raccourci pratique : améliore un peu l'environnement
    /// </summary>
    public void ImproveEnvironment(float amount)
    {
	Debug.Log(amount);
        SetTargetPollution(pollution - amount);
	Debug.Log("Target pollution setted.");
    }
}

