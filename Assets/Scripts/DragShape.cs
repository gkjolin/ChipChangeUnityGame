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

	void OnMouseDown()
	{
		if(!isReady) return;
		
		if(!isActivated)
		{
			isActivated = true;
			screenPoint = Camera.main.WorldToScreenPoint(thisTransform.position);
		}
	}
	
	void OnMouseDrag()
	{
		if(!isReady) return;
		if(isActivated)
		{
			isDragging = true;
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
			thisTransform.position = curPosition;
		}
	}
	
}