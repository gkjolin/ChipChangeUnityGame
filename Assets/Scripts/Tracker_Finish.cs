using UnityEngine;
using System.Collections;

public class Tracker_Finish : MonoBehaviour {
	
	public Tracker_Spawn trackerSpawn;		// Need this to know how many chips we start with
	public int chipsNeeded;					// number of chips to finish the level
	public int goToLevel;
	public float cameraMoveDelaySecs = 3f;
	public Transform cameraPositionsParent;		// get an array of this gameobjects children to move camera to next level
	
	Transform[] cameraPositionsArray;
	Text_Typewriter textEffect;					// use this class to show and remove our text
	int currentLevelInt;					// Tracks what level we are on
	int chipsCount;							// Tracks remaining chips needed for this level
	bool isActivated;
	
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
	
	void Start () 
	{
		textEffect = GetComponent<Text_Typewriter>();
		chipsCount = chipsNeeded;
		textEffect.ShowText ("need " + chipsCount + " more", 0f);
		// make an array to hold the cameraPositions transform. Will use to move camera to next level
		cameraPositionsArray = new Transform[cameraPositionsParent.childCount];
		for (var i=0; i < cameraPositionsParent.childCount; i++){
			cameraPositionsArray[i] = cameraPositionsParent.GetChild(i);
		}
		Camera.main.transform.position = cameraPositionsArray[goToLevel].position;

	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		if (isActivated) return;

		if (col.gameObject.CompareTag("Chip"))
		{
			isActivated = true;
			Invoke("ResetIsActivated", 0.1f);
			// Tell chip to despawn
			col.gameObject.SendMessage("FinishedDespawn",SendMessageOptions.DontRequireReceiver);
			// Reduce chipsNeeded count by 1
			chipsCount -= 1;
			if (chipsCount == 0)
			{
				Messenger.Invoke("levelComplete");				// tell the world the level is complete
			}
			else
			{
				textEffect.TextToShow = "need " + chipsCount + " more";
				textEffect.RemoveText (true);
			}
		}
	}

	void ResetIsActivated()
	{
		isActivated = false;
	}

	void ResetChipCountText()				// Used after level complete once we are to the new level
	{
		textEffect.TextToShow = "need " + chipsCount + " more";
	}

	void OnReset()
	{
		chipsCount = chipsNeeded;
		textEffect.TextToShow = "need " + chipsCount + " more";
		textEffect.RemoveText (true);
	}
	
	void OnLevelComplete()
	{
		textEffect.TextToShow = "level complete";
		textEffect.RemoveText (true);
		textEffect.Invoke("RemoveText",3f);
		chipsCount = chipsNeeded;
		Invoke("ResetChipCountText",6.5f);
		textEffect.Invoke("ShowText",7f);

		currentLevelInt += 1;
		// move camera (delayed) to next level position
		LeanTween.move( Camera.main.gameObject, cameraPositionsArray[currentLevelInt].position, 
			4f, new object[]{ "delay", cameraMoveDelaySecs, "ease", LeanTweenType.easeInOutSine});
	}


}
