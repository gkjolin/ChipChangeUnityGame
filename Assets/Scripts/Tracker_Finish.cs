using UnityEngine;
using System.Collections;

public class Tracker_Finish : MonoBehaviour {
	
	public Tracker_Spawn trackerSpawn;		// Need this to know how many chips we start with
	public int chipsNeeded;					// number of chips to finish the level

	public Transform cameraPositionsParent;		// get an array of this gameobjects children to move camera to next level
	
	Transform[] cameraPositionsArray;
	Text_Typewriter textEffect;					// use this class to show and remove our text
	int chipCount;								// Tracks remaining chips needed for this level
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
		chipCount = chipsNeeded;
		Invoke("Setup",0.1f);
	}

	void Setup ()
	{
		if (_Manager.currentLevel != 0) textEffect.ShowTextDelayed("need " + chipCount + " more", 3f);		// Dont show this text if we are on the menu (level 0)
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
			chipCount -= 1;
			if (chipCount == 0)
			{
				Messenger.Invoke("levelComplete");				// tell the world the level is complete
			}
			else
			{
				textEffect.TextToShow = "need " + chipCount + " more";
				textEffect.RemoveText (true);
			}
		}
	}

	void ResetIsActivated()
	{
		isActivated = false;
	}
	
	void OnReset()
	{
		chipCount = chipsNeeded;
		textEffect.TextToShow = "need " + chipCount + " more";
		textEffect.RemoveText (true);
	}
	
	void OnLevelComplete()
	{
		textEffect.TextToShow = "level complete";
		textEffect.RemoveText (true);		// Remove current text and show "level complete"
		textEffect.RemoveText (3f);			// Then in 3 seconds remove "level complete"
		chipCount = chipsNeeded;			// Reset chipCount
		textEffect.ShowTextDelayed ("need " + chipCount + " more", 7f);		// Show the text again in 7 seconds
	}




}
