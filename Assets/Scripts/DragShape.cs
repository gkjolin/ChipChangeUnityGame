using UnityEngine;
using System.Collections;


public class DragShape : MonoBehaviour
{
	private Transform thisTransform;
	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 originalPos;
	private bool isReady;
	private bool isActivated;
	private RaycastHit2D[] hit2DArray = new RaycastHit2D[1];
	[System.NonSerialized]
	public bool isDragging; 
	
	
	void Start()
	{
		thisTransform = transform;
		originalPos = thisTransform.position;
		Invoke("SetIsReady",3f);
	}

	void SetIsReady()
	{
		isReady = true;
	}

	void Update()
	{
		if(!isReady) return;
		
		
		
		if (Input.GetMouseButtonDown(0) && !isActivated)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Physics2D.GetRayIntersectionNonAlloc(ray,hit2DArray, Mathf.Infinity);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
		//	RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hit.collider != null && hit.collider.transform == thisTransform)
			{
				isActivated = true;
				screenPoint = Camera.main.WorldToScreenPoint(thisTransform.position);
			}
		}
		
		if (Input.GetMouseButtonUp(0) && isActivated)
		{
			isActivated = false;
			isDragging = false;
		}
		
		if (isActivated)
		{
			isDragging = true;
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
			thisTransform.position = curPosition;
		}
	}

}