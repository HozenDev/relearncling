using UnityEngine;

public class XRUIWidthController : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform parentRect;

    public float parent_width;

    void Awake()
    {
	rectTransform = GetComponent<RectTransform>();
	parentRect = transform.parent.GetComponent<RectTransform>();

	if (parentRect != null)
	{
	    parent_width = parentRect.rect.width;
	}
    }
    
    void Update()
    {
	if (PollutionController.Instance == null)
            return;

        float pollution = PollutionController.Instance.pollution;

	float width = parent_width * pollution; // pollution from 1 to 0
	
	Vector2 size = rectTransform.sizeDelta;
	size.x = width;
	rectTransform.sizeDelta = size;
    }
}
