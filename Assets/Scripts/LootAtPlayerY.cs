using UnityEngine;

public class LookAtPlayerY: MonoBehaviour
{
    public Transform m_toLookAt;

    void Update()
    {
	if (m_toLookAt == null) return;

	Vector3 lookPos = m_toLookAt.position - transform.position;
	lookPos.y = 0;

	if (lookPos.sqrMagnitude > 0.001f)
	{
	    Quaternion targetRotation = Quaternion.LookRotation(lookPos);
	    targetRotation *= Quaternion.Euler(0, 180f, 0);

	    transform.rotation = targetRotation;
	}
    }
}
