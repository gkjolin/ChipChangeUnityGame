using UnityEngine;
using System.Collections;

public class Chip_Spawner : MonoBehaviour {

	public GameObject shape;
	
	private Drag_Shape dragShape;
	private Transform thisTransform;
	private Vector3 originalPosition;
	private Vector3 spawnPoint;
	private bool isReady;
	private bool isActivated;
	private int spawnLayer;
	
	void Start()
	{
		// cache these things for performance reasons
		thisTransform = transform;
		originalPosition = new Vector3 (thisTransform.position.x+1.5f,thisTransform.position.y,thisTransform.position.z);
		// grab dragshape script, will need it to check if we are dragging this shape
		dragShape = GetComponent<Drag_Shape>();
		// setup a bitmask of the spawnlayer for a raycast check (can only spawn while within a spawn collider)
		spawnLayer = 1 << 11;
		// Wait 2 seconds before doing anything
		Invoke("SetIsReady", 2f);
	}
	
	void SetIsReady()
	{
		isReady = true;
	}
	
	void Update()
	{
		// If the user clicks down on the mouse and we havent done this before
		if(Input.GetMouseButtonUp(0) && isReady && !isActivated && dragShape.isDragging)
		{
			// Use ScreenPointToRay to get world position of mouse, this will be the spawnPoint
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray,1000f,spawnLayer);
			
			// If the mouse position hits a spawn collider spawn our shape
			if (hit.collider != null)
			{
				isActivated = true;
				spawnPoint = ray.origin + (ray.direction * -Camera.main.gameObject.transform.position.z);
		    	spawnPoint = new Vector3(spawnPoint.x,spawnPoint.y,0f);
			
				// transition tween moving spawner gameobject to spawn position of shape gameobject 
				LeanTween.move( gameObject, spawnPoint, .7f, new object[]{ "ease",LeanTweenType.easeInSine});
			
				Invoke("SpawnShapeAndDisableThis",.75f);
			}
			else
			{
				// Move the spawn shape back to its original position so the user can try and drag it again
				LeanTween.move( gameObject, originalPosition, .7f, new object[]{ "ease",LeanTweenType.easeInSine});
			}
		}
	}
	
	void SpawnShapeAndDisableThis()
	{
		// Spawn the actual shape where the mouse is
		Instantiate(shape, spawnPoint, transform.rotation);
		// Tell the Spawn Tracker we have successfully spawned
		transform.parent.SendMessage("SpawnedShape",SendMessageOptions.DontRequireReceiver);
		// Disable this object (Avoid destroy for Garbage collection reasons)
		this.gameObject.SetActive(false);
	}
}