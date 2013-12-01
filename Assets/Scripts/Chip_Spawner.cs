using UnityEngine;
using System.Collections;

public class Chip_Spawner : MonoBehaviour {

	public GameObject prefab;

	Drag_Chip dragChip;
	Transform thisTransform;
	Transform chipSpawned;
	Vector3 originalLocalPosition;
	Vector3 onScreenPosition;
	Vector3 spawnPoint;
	bool isReady;
	bool isActivated;
	bool isOnScreen;
	bool isChangingLevels;
	int spawnLayer;
	GameObjectPool chipPool;
	
	void OnEnable()			
    {
        Messenger.AddListener("reset", OnReset);			// Register to the reset event on enable
		Messenger.AddListener("levelComplete", OnLevelComplete);			
    }
	
	void OnDisable()
    {
        Messenger.RemoveListener("reset", OnReset);			// Always make sure to unregister the event on disable
		Messenger.RemoveListener("levelComplete", OnLevelComplete);			
    }
	
	void Start()
	{
		thisTransform = transform;
		chipPool = GameObjectPool.GetPool("Chip_Pool");		//Setup the pool for spawning chips	
		originalLocalPosition = thisTransform.localPosition;
		onScreenPosition = new Vector3 (thisTransform.localPosition.x + _Manager.chipSpawnTweenDist, 
		                                thisTransform.localPosition.y, thisTransform.localPosition.z);
		// reference Drag_Chip script, will need it to check if we are dragging this shape
		dragChip = GetComponent<Drag_Chip>();
		// setup a bitmask of the spawnlayer for a raycast check (can only spawn while within a spawn collider)
		spawnLayer = 1 << 11;
		// Wait 2 seconds before doing anything
		Invoke("SetIsReady", 2f);
	}
	
	void SetIsReady()
	{
		isReady = true;
		isChangingLevels = false;
	}

	void Update()
	{
		if (!Application.isEditor)		// Avoid this check when in editor because OnBecameVisible()
		{								// doesnt work well while using separate editor window
			if(!isOnScreen) return;
		}
		// If the user clicks down on the mouse and we havent done this before
		if(Input.GetMouseButtonUp(0) && isReady && !isActivated && dragChip.isDragging && !dragChip.hasDragged)
		{
			// Use ScreenPointToRay to get world position of mouse, this will be the spawnPoint
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray,1000f,spawnLayer);
			
			// If the mouse position hits a spawn collider spawn our chip
			if (hit.collider != null && !isChangingLevels)
			{
				isActivated = true;
				spawnPoint = ray.origin + (ray.direction * - Camera.main.gameObject.transform.position.z);
		    	spawnPoint = new Vector3 (spawnPoint.x, spawnPoint.y, 0f);
			
				// transition tween moving this spawner to spawn position of chip gameobject 
				LeanTween.move( gameObject, spawnPoint, .7f, new object[]{ "ease",LeanTweenType.easeInSine});
				Invoke("SpawnShapeAndMoveThis",.75f);
			}
			else
			{
				// Move the spawn shape back to its original position so the user can try and drag it again
				LeanTween.moveLocal( gameObject, onScreenPosition, 0.7f, new object[]{ "ease",LeanTweenType.easeInSine});
				dragChip.Invoke("ResetHasDragged", 0.71f);		// Once the tween is finished, activate the spawner again
			}
		}
	}
	
	void SpawnShapeAndMoveThis()
	{
		// Spawn the actual shape where the mouse is
		chipPool.GetInstance(thisTransform.position);
		// Announce that we have successfully spawned
		Messenger.Invoke ("chipSpawned");
		dragChip.hasDragged = false;
		dragChip.isDragging = false;
		// move this gameobject to it's original position off screen
		thisTransform.localPosition = originalLocalPosition;
		isActivated = false;
		isOnScreen = false;
	}
	
	void OnReset()
	// Resets this script completely for a new level or user reset
	{
		isActivated = false;
	}
	
	void OnLevelComplete()
	// Resets this script completely for a new level or user reset
	{
		isActivated = false;
		isChangingLevels = true;					// We wont spawn chips till the next level
		Invoke ("SetIsReady", 7f);					// This will let us spawn chips in 7 seconds
	}
	
	void OnBecameVisible()
	{
		isOnScreen = true;
	}
	
	void OnBecameInvinsible()
	{
		isOnScreen = false;
	}
}