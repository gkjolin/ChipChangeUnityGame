using UnityEngine;
using System.Collections;

public class Text_Reset : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
			
			// If it hit this gameObject, we will activate the dragging
			if(hit.collider != null && hit.collider.transform == transform)
			{
				Messenger.Invoke ("reset");
			}
		}
	}
}
