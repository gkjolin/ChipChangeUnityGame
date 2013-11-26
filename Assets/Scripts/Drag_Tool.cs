using UnityEngine;
using System.Collections;

public class Drag_Tool : MonoBehaviour
{
	public bool rotateInsteadOfDrag;
	public bool limitVerticalDrag;
	public bool limitHorizontalDrag;
	
	public float maxVerticalDrag;
	public float minVerticalDrag;
	public float maxHorizontalDrag;
	public float minHorizontalDrag;
	
	private Transform thisTransform;
	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 originalPosition;
	private float angleOffset;
	private bool isReady;
	private bool isActivated;
	[System.NonSerialized]
	public bool isDragging; 
	
	
	void Start()
	{
		thisTransform = transform;
		originalPosition = thisTransform.position;
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
				if (rotateInsteadOfDrag)
				{
					Vector3 v3 = Input.mousePosition - screenPoint;
					angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(v3.y, v3.x))  * Mathf.Rad2Deg;
				}
			}
		}
		
		// Stop dragging when the mouse is unclicked
		if (Input.GetMouseButtonUp(0) && isActivated)
		{
			isActivated = false;
			isDragging = false;
		}
		
		// If if the mouse is still clicked, drag this gameObject to its location
		if (isActivated)
		{
			isDragging = true;
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
			
			Vector3 newPosition = thisTransform.position;
			
			if (rotateInsteadOfDrag)
			{
				Vector3 v3 = Input.mousePosition - screenPoint;
				float angle = Mathf.Atan2(v3.y, v3.x) * Mathf.Rad2Deg;
				transform.eulerAngles = new Vector3(0,0,angle+angleOffset); 
			}
			
			if (limitHorizontalDrag)
			{
				if (curPosition.x <= originalPosition.x + maxHorizontalDrag
					&& curPosition.x >= originalPosition.x - minHorizontalDrag)
				{
					newPosition.x = curPosition.x;
				}
			}
			else
			{
				newPosition.x = curPosition.x;
			}
			
			if (limitVerticalDrag)
			{
				print(originalPosition.y + maxVerticalDrag);
				print(originalPosition.y - minVerticalDrag);	
				if (curPosition.y <= originalPosition.y + maxVerticalDrag
					&& curPosition.y >= originalPosition.y - minVerticalDrag)
				{
					newPosition.y = curPosition.y;
				}
			}
			else
			{
				newPosition.y = curPosition.y;
			}
			
			thisTransform.position = newPosition;
		}
	}

}