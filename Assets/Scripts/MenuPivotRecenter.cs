using UnityEngine;

public class MenuPivotRecenter : MonoBehaviour
{
    [Header("UI Roots")]
    public RectTransform menusRoot;          // The object that contains MainMenu + SubMenu
    public RectTransform[] menuRects;        // Assign MainMenu, SubMenu here in the Inspector

    private readonly Vector3[] _corners = new Vector3[4];

    public Transform only_main;
    public Transform both_menu;

    /// <summary>
    /// Call this whenever you toggle a submenu on/off.
    /// </summary>
    public void RecenterOnActiveMenus()
    {
        bool hasAny = false;
        Vector3 min = Vector3.zero;
        Vector3 max = Vector3.zero;

        foreach (var rt in menuRects)
        {
            if (rt == null || !rt.gameObject.activeInHierarchy)
                continue;

            rt.GetWorldCorners(_corners);

            if (!hasAny)
            {
                min = max = _corners[0];
                for (int i = 1; i < 4; i++)
                {
                    min = Vector3.Min(min, _corners[i]);
                    max = Vector3.Max(max, _corners[i]);
                }
                hasAny = true;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    min = Vector3.Min(min, _corners[i]);
                    max = Vector3.Max(max, _corners[i]);
                }
            }
        }

        if (!hasAny)
            return; // nothing active, nothing to do

        // Center of all ACTIVE menus
        Vector3 center = (min + max) * 0.5f;

        // Move the pivot to the center, but keep the menus visually in place
        Vector3 oldPivotPos = transform.position;
        Vector3 delta = center - oldPivotPos;

        // Move pivot
        transform.position += delta;

        // Move menus back so they stay where they were
        if (menusRoot != null)
        {
            menusRoot.position -= delta;
        }
    }
}
