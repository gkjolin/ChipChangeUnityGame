using UnityEngine;
using System.Collections;

public class Tracker_Finish : MonoBehaviour {
	
	public Tracker_Spawn trackerSpawn;		// Need this to know how many chips we start with
	public Transform cameraPositionsParent;		// get an array of this gameobjects children to move camera to next level
	
	Transform[] cameraPositionsArray;
	Text_Typewriter textEffect;					// use this class to show and remove our text
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
		Invoke("Setup",0.1f);
	}

	void Setup ()
	{
		if (_Manager.currentLevel != 0) textEffect.ShowTextDelayed("need " + _Manager.currentChipsNeededCount + " more", 3f);		// Dont show this text if we are on the menu (level 0)
	}

	void OnTriggerEnter2D (Collider2D col) 
	{
		if (isActivated) return;

		if (col.gameObject.CompareTag("Chip"))
		{
			isActivated = true;
			_Manager.audioFinish.Play();
			Invoke("ResetIsActivated", 0.1f);
			// Tell chip to despawn
			col.gameObject.SendMessage("FinishedDespawn",SendMessageOptions.DontRequireReceiver);
			// Reduce _Manager.currentChipsNeededCount count by 1
			_Manager.currentChipsNeededCount -= 1;
			if (_Manager.currentChipsNeededCount == 0)
			{
				Messenger.Invoke("levelComplete");				// tell the world the level is complete
			}
			else
			{
				textEffect.TextToShow = "need " + _Manager.currentChipsNeededCount + " more";
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
		textEffect.TextToShow = "need " + _Manager.currentChipsNeededCount + " more";
		textEffect.RemoveText (true);
	}
	
	void OnLevelComplete()
	{
		textEffect.TextToShow = "level complete";
		textEffect.RemoveText (true);		// Remove current text and show "level complete"
		textEffect.RemoveText (3f);			// Then in 3 seconds remove "level complete"
		textEffect.ShowTextDelayed ("need " + _Manager.currentChipsNeededCount + " more", 7f);		// Show the text again in 7 seconds
	}




}
