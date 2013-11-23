using UnityEngine;
using System.Collections;

public class SpawnerLogic : MonoBehaviour {

	public GameObject shape;
	
	private DragShape dragShape;
	private Transform thisTransform;
	private Vector3 originalPosition;
	private Vector3 spawnPoint;
	private bool isReady;
	private bool isActivated;
	private int spawnLayer;
	
	void Start()
	{
		thisTransform = transform;
		originalPosition = thisTransform.position;
		dragShape = GetComponent<DragShape>();
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
				LeanTween.move( gameObject, originalPosition, .7f, new object[]{ "ease",LeanTweenType.easeInSine});
			}
		}
	}
	
	void SpawnShapeAndDisableThis()
	{
		Instantiate(shape, spawnPoint, transform.rotation);
		this.gameObject.SetActive(false);
	}
}