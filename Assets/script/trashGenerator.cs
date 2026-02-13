using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class trashGenerator : MonoBehaviour
{
    public GameObject[] prefabs;          // your random prefabs
    public Transform spawnPoint;          // where to spawn
    private GameObject currentObject;     // currently spawned instance
    private bool objectInsideZone = false;

    public List<GameObject> spawnedObjects = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Spawn a random prefab
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            currentObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            spawnedObjects.Add(currentObject);

            objectInsideZone = true;

            // Wait until the object exits the zone
            yield return new WaitUntil(() => objectInsideZone == false);

            // Optional: wait before next spawn
            yield return new WaitForSeconds(0.5f);
        }
    }

    // When the spawned object ENTERS the zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == currentObject)
        {
            objectInsideZone = true;
        }
    }

    // When it LEAVES the zone
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentObject)
        {
            objectInsideZone = false;
        }
    }

    public void ClearAll()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        spawnedObjects.Clear();
        
        
        PollutionController.Instance.SetTargetPollution(1);
        ScoreManager.Instance.RemoveScore(ScoreManager.Instance.score);
        objectInsideZone = false;

    }
}