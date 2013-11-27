using UnityEngine;
using System.Collections;

public class Tracker_Finish : MonoBehaviour {
	
	public Tracker_Spawn trackerSpawn;		// Need this to know how many chips we start with
	public int chipsNeeded;					// number of chips to finish the level
	public Transform cameraPositionsParent;		// get an array of this gameobjects children to move camera to next level
	
	Transform[] cameraPositionsArray;
	TextMesh needChipsText;					// "Needs x" Tracks how many Chips we need to move to next level
	int currentLevelInt;					// Tracks what level we are on
	
	void Start () 
	{
		needChipsText = GetComponent<TextMesh>();
		// Set chipsNeeded text
		needChipsText.text = chipsNeeded + "chips";
		// make an array to hold the cameraPositions transform. Will use to move camera to next level
		cameraPositionsArray = new Transform[cameraPositionsParent.childCount];
		for (var i=0; i < cameraPositionsParent.childCount; i++){
			cameraPositionsArray[i] = cameraPositionsParent.GetChild(i);
		}
	}
	

	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.gameObject.CompareTag("Chip"))
		{
			// Tell chip to despawn
			col.gameObject.SendMessage("FinishedDespawn",SendMessageOptions.DontRequireReceiver);
			// Reduce chipsNeeded count by 1
			chipsNeeded -= 1;
			// Refresh text
			needChipsText.text = chipsNeeded + " chips";
			if (chipsNeeded == 0)
				LevelComplete();
		}
	}
	
	void LevelComplete()
	{
		currentLevelInt += 1;
		LeanTween.move( Camera.main.gameObject, cameraPositionsArray[currentLevelInt].position, 4f, new object[]{ "ease",LeanTweenType.easeInOutSine});
	}
}
