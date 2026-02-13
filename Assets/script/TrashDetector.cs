using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDetector : MonoBehaviour
{
    // 1 : recyclable
    // 2 : Non recyclable
    // 3 : bio degradable
    // 4 : glass
    public int zoneNumber; // The correct number for this zone
    public int reward = 2;
    public int penalty = 1;

    public AudioClip wrong_sound; // Assign your AudioClip in the Inspector
    public AudioClip good_sound; // Assign your AudioClip in the Inspector
    private AudioSource audioSource;

    public trashGenerator generator; // Reference to the trashGenerator

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        typeOfTrash obj = other.GetComponent<typeOfTrash>();

        if (obj != null)
        {
            if (obj.typeNumber == zoneNumber)
            {
                // Correct sorting
                ScoreManager.Instance.AddScore(reward);
		        PollutionController.Instance.ImproveEnvironment(reward/30.0f);

                audioSource.PlayOneShot(good_sound);
            }
            else
            {
                // Wrong sorting
                ScoreManager.Instance.RemoveScore(penalty);
		        PollutionController.Instance.ImproveEnvironment(-penalty/ 30.0f);

                audioSource.PlayOneShot(wrong_sound);
            }

            generator.spawnedObjects.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
