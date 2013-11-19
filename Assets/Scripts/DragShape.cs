using UnityEngine;
using System.Collections;


public class DragShape : MonoBehaviour
{
	private Transform thisTransform;
	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 originalPos;
	private bool isReady;

	void Start()
	{
		thisTransform = transform;
		originalPos = thisTransform.position;
		Invoke("SetIsReady",2f);
	}

	void SetIsReady()
	{
		isReady = true;
	}

	void OnMouseDown()
	{
		if(isReady)
		{
			screenPoint = Camera.main.WorldToScreenPoint(thisTransform.position);
			offset = thisTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
	}
	
	void OnMouseDrag()
	{
		if(isReady)
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint), offset;
			thisTransform.position = curPosition;
		}
	}
	
}