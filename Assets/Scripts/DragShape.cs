using UnityEngine;
using System.Collections;


public class DragShape : MonoBehaviour
{
	private Transform thisTransform;
	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 originalPos;

	void Start()
	{
		thisTransform = transform;
		originalPos = thisTransform.position;
	}

	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(thisTransform.position);
		offset = thisTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		
	}
	
	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint), offset;
		thisTransform.position = curPosition;
		
	}
	
}