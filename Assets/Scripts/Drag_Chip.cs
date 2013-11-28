using UnityEngine;
using System.Collections;

public class Drag_Chip : MonoBehaviour
{
	Transform thisTransform;
	Vector3 screenPoint;
	Vector3 offset;
	float angleOffset;
	bool isReady;
	bool isActivated;
	[System.NonSerialized]
	public bool isDragging; 
	
	void OnEnable()			
    {
        Messenger.AddListener("reset", OnReset);			// Register to the reset event on enable
    }
	
	void OnDisable()
    {
        Messenger.RemoveListener("reset", OnReset);			// Always make sure to unregister the event on disable
    }
	
	void Start()
	{
		thisTransform = transform;
		// Wait 2 seconds before doing anything
		Invoke("SetIsReady",2f);
	}

	void SetIsReady()
	{
		isReady = true;
	}

	void Update()
	{
		if(!isReady) return;
		
		// Cant use OnMouseDown() etc in flash. Would have been easier and avoided raycasting
		if (Input.GetMouseButtonDown(0) && !isActivated)
		{
			// Shoot a ray at mousePosition to see what 2d collider is hit
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
			
			// If it hit this gameObject, we will activate the dragging
			if(hit.collider != null && hit.collider.transform == thisTransform)
			{
				isActivated = true;
				screenPoint = Camera.main.WorldToScreenPoint(thisTransform.position);
			}
		}
		
		// Stop dragging when the mouse is unclicked
		if (Input.GetMouseButtonUp(0) && isActivated)
		{
			isActivated = false;
			Invoke("ResetIsDragging",.1f);
		}
		
		// If if the mouse is still clicked, drag this gameObject to its location
		if (isActivated)
		{
			
			isDragging = true;
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
			
			thisTransform.position = curPosition;
		}
	}
	
	void ResetIsDragging()
	{
		isDragging = false;	
	}
	
	
	// Resets this gameobject completely. For next level and user reset
	void OnReset()
	{
		isActivated = false;	
	}

}