using UnityEngine;
using UnityEngine.UI;

public class ToggleSubMenu : MonoBehaviour
{
    [Header("Menu References")]
    public GameObject subMenu;

    /// <summary>
    /// Call this from the Button OnClick event.
    /// </summary>
    public void Toggle()
    {
        if (subMenu == null) return;

        // Toggle submenu active state
        bool newState = !subMenu.activeSelf;
        subMenu.SetActive(newState);
    }
}
