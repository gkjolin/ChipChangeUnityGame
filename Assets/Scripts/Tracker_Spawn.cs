using UnityEngine;
using System.Collections;
using System.Linq;

public class Tracker_Spawn: MonoBehaviour {
	
	Transform[] childrenArray;
	TextMesh spawnCountText;
	int currentSpawnCount;
	Vector3 spawnerMovePosition;
	
	void Start () 
	{
		// Linq statement that selects all the children of this gameobject *Oops LINQ doesnt work in flash which im going to build to
		//childrenArray = GetComponentsInChildren<Transform>().Except(new [] { transform }).Select(t=>t).ToArray();
		
		// Make an array of Transforms of this gameobjects children
		childrenArray = new Transform[transform.childCount];
		// add each child to the array
		for (var i=0; i < transform.childCount; i++){
			childrenArray[i] = transform.GetChild(i);
		}
		
		// Get the text mesh of this gameobject so we can update the score
		spawnCountText = GetComponent<TextMesh>();
		// Give unity a bit of time to setup (usually a good idea)
		Invoke("Setup",1f);
	}
	
	void Setup()
	{
		// Get the current number of children of this gameobject
		currentSpawnCount = childrenArray.Length;
		// Setup the spawn count text
		spawnCountText.text = "x" + (currentSpawnCount - 1);
		// Setup the position where spawn shapes will slide into (they start offscreen)
		spawnerMovePosition = new Vector3 (childrenArray[0].position.x + 1.5f, childrenArray[0].position.y, childrenArray[0].position.z);
		// Move the first child spawn shape into position
		LeanTween.move( childrenArray[0].gameObject, spawnerMovePosition, .7f, new object[]{ "ease",LeanTweenType.easeOutSine});
	}
	
	void SpawnedShape () 
	{
		// This method is called by the spawner shapes when they spawn a shape
		// Reduce our spawn count by 1.
		if (currentSpawnCount != 0)
			currentSpawnCount -= 1;
		// figure out which child we need to move in next.
		int spawnerToMove = childrenArray.Length - currentSpawnCount;
		// move em in
		if (spawnerToMove < childrenArray.Length) 
			LeanTween.move( childrenArray[spawnerToMove].gameObject, spawnerMovePosition, .7f, new object[]{ "ease",LeanTweenType.easeOutSine});
		if (currentSpawnCount >= 1)
			// update spawn count text
			spawnCountText.text = "x" + (currentSpawnCount - 1);
	}
}
