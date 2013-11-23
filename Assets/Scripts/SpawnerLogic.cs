using UnityEngine;
using System.Collections;

public class SpawnerLogic : MonoBehaviour {

	public GameObject shape;
	
	private DragShape dragShape;
	private Transform thisTransform;
	private Vector3 spawnPoint;
	private bool isReady;
	private bool isActivated;
	
	void Start()
	{
		thisTransform = transform;
		dragShape = GetComponent<DragShape>();
		Invoke("SetIsReady", 3f);
	}
	
	void SetIsReady()
	{
		isReady = true;
	}
	
	void Update()
	{
		if(Input.GetMouseButtonUp(0) && isReady && !isActivated && dragShape.isDragging)
		{
			isActivated = true;
			
			// Use ScreenPointToRay to get world position of mouse, this will be the spawnPoint
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			spawnPoint = ray.origin + (ray.direction * -Camera.main.gameObject.transform.position.z);
		    spawnPoint = new Vector3(spawnPoint.x,spawnPoint.y,0f);
			
			// transition tween moving spawner gameobject to spawn position of shape gameobject 
			LeanTween.move( gameObject, spawnPoint, .7f, new object[]{ "ease",LeanTweenType.easeInSine});
			
			Invoke("SpawnShapeAndDisableThis",.75f);
		}
	}
	
	void SpawnShapeAndDisableThis()
	{
		Instantiate(shape, spawnPoint, transform.rotation);
		this.gameObject.SetActive(false);
	}
}