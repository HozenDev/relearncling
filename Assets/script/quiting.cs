using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quiting : MonoBehaviour
{
    public void QuitGame()
    {
        // This will close the application when built.
        // In the Unity Editor, it will stop Play Mode.
        Application.Quit();

        // Optional: Log a message to the console for debugging
        Debug.Log("Game is quitting...");

        // For quitting in the editor (optional, and only for editor functionality)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
