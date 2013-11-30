using UnityEngine;
using System.Collections;

public class Text_Reset : MonoBehaviour {
	
	private bool isActivated;
	
	void Update ()
	{
		if (isActivated) return;
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
			
			if(hit.collider != null && hit.collider.transform == transform)
			{
				isActivated = true;
				Invoke("ResetIsActivated",2f);
				Messenger.Invoke ("reset");
			}
		}
	}
	
	void ResetIsActivated()
	{
		isActivated = false;	
	}
}
