using UnityEngine;
using System.Collections;
using System.Linq;

public class Tracker_Spawn: MonoBehaviour {

	GameObject[] childrenArray;
	Text_Typewriter textEffect;
	Vector3 spawnerMovePosition;
	
	void OnEnable()			
    {
        Messenger.AddListener ("reset", OnReset);			// Register to the reset event on enable
		Messenger.AddListener ("chipSpawned", OnChipSpawned);
		Messenger.AddListener ("levelComplete", OnLevelComplete);
    }
	
	void OnDisable()
    {
        Messenger.RemoveListener ("reset", OnReset);			// Always make sure to unregister the event on disable
		Messenger.RemoveListener ("chipSpawned", OnChipSpawned);
		Messenger.RemoveListener ("levelComplete", OnLevelComplete);
    }
	
	void Start () 
	{
		// Linq statement that selects all the children of this gameobject *Oops LINQ doesnt work in flash which im going to build to
		//childrenArray = GetComponentsInChildren<Transform>().Except(new [] { transform }).Select(t=>t).ToArray();
		
		// Make an array of Transforms of this gameobjects children, which are all the chip spawners
		childrenArray = new GameObject[transform.childCount];
		// add each child to the array
		for (var i=0; i < transform.childCount; i++){
			childrenArray[i] = transform.GetChild(i).gameObject;
		}
		
		// Get the text mesh of this gameobject so we can update the score
		textEffect = GetComponent<Text_Typewriter>();
		// Give unity a bit of time to setup (usually a good idea)
		Invoke("Setup",3f);
	}
	
	void Setup()
	{
		// Setup the spawn count text
		if (_Manager.currentLevel != 0) textEffect.ShowText("x" + (_Manager.currentSpawnChipCount - 1));
		// Setup the position where Chip_Spawner will slide into (they start offscreen)
		spawnerMovePosition = new Vector3 (childrenArray[0].transform.localPosition.x + _Manager.chipSpawnTweenDist, 
		                                   childrenArray[0].transform.localPosition.y, childrenArray[0].transform.localPosition.z);
		// Move the first child spawn shape into position
		MoveChipSpawnerToScreen(0);
	}
	
	void MoveChipSpawnerToScreen(int spawnerToMove)
	{
		LeanTween.moveLocal( childrenArray[spawnerToMove], spawnerMovePosition, .7f, new object[]{ "ease",LeanTweenType.easeOutSine});
	}

	void LevelCompleteUpdateText()
	{
		textEffect.ShowTextDelayed ("x" + (_Manager.totalSpawnChipCount - 1), 7f);			// Show text again in 7 seconds
	}
	
	void OnChipSpawned () 
	{
		// This method is called by the Chip_Spawner when they spawn a chip
		// Reduce our spawn count by 1.
		if (_Manager.currentSpawnChipCount != 0)
			_Manager.currentSpawnChipCount -= 1;
		// figure out which child we need to move in next.
		int spawnerToMove = childrenArray.Length - _Manager.currentSpawnChipCount;
		// move em in (making sure the spawnerToMove is within the childrenArray size)
		if (spawnerToMove < childrenArray.Length)
		{
			MoveChipSpawnerToScreen(spawnerToMove);
		}
		if (_Manager.currentSpawnChipCount >= 1)
		{
			// update spawn count text
			textEffect.TextToShow = "x" + (_Manager.currentSpawnChipCount - 1);
			textEffect.RemoveText(true);
		}
	}
	
	void OnReset()
	{
		// Bring a new chip spawner on screen if the current spawn count is zero
		if (_Manager.currentSpawnChipCount <= 0)
		{
			MoveChipSpawnerToScreen(0);
		}
		// Reset spawn count and text        

		textEffect.TextToShow = "x" + (_Manager.totalSpawnChipCount - 1);
		textEffect.RemoveText (true);
		
	}
	
	void OnLevelComplete()
	{
		if (_Manager.currentSpawnChipCount <= 0)
		{
			MoveChipSpawnerToScreen(0);
		}
		textEffect.RemoveText ();				// Clear text
		Invoke("LevelCompleteUpdateText", .1f);	
	}

}
