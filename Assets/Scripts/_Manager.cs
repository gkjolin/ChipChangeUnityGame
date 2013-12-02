using UnityEngine;
using System.Collections;

public class _Manager : MonoBehaviour {

	public int levelToStart;							// These static variables can be accessed by any class via _Manager.levelToStart
	public int[] chipsNeededPerLevel;		// A public array so we can change the chipsNeeded requirement per level.			
	public int[] totalSpawnsPerLevel;		// A public array so we can change the total spawns allowed per level.

	public static float camMoveDelaySecs = 3f;			// time it takes for before moving camera to next level
	public static float camTransitionSecs = 4f;			// time it takes for camera to move from level to level
	public static AudioSource audioBlip0;				// Text_Typewriter calls this during typing effect
	public static AudioSource audioBlip1;			
	public static AudioSource audioBlip2;			
	public static AudioSource audioBlip3;	
	public static int currentChipsNeededCount;			// How many remaining chips are required to complete the level
	public static int totalSpawnChipCount;				// How many chips are really allowed to spawn in this level
	public static int currentSpawnChipCount;			// How many chips are left to spawn in this level
	public static float chipSpawnTweenDist = 7.5f;				// How far the chip spawners shift to move onscreen.
	public static int currentLevel;						// What level number are we on? starting with 0
	public static int score;							// overall score .... not implemented

	void Start () 
	{
		if (Application.isEditor)						// if we are in unity we skip to levelToStart
		{
			currentLevel = levelToStart;			
			currentChipsNeededCount = chipsNeededPerLevel [currentLevel];
			totalSpawnChipCount = totalSpawnsPerLevel[currentLevel];
			currentSpawnChipCount = totalSpawnChipCount;
		}

		AudioSource[] aSources = GetComponents<AudioSource>();
		audioBlip0 = aSources[0];
		audioBlip1 = aSources[1];
		audioBlip2 = aSources[2];
		audioBlip3 = aSources[3];
	}

	public static void LoadSavedLevel ()
	{
		currentLevel = PlayerPrefs.GetInt("Current Level");			// Load the saved level to start at
		if (currentLevel == 0) currentLevel ++;						// If the playerpref returns 0, nothing is saved, go to first level
		if (currentLevel > 6) currentLevel = 6; 					//TEMPPPPPP BUG FIX
	}
	
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

	public static void Mute()
	{
		audioBlip0.mute = true;
		audioBlip1.mute = true;
		audioBlip2.mute = true;
		audioBlip3.mute = true;
	}

	public static void UnMute()
	{
		audioBlip0.mute = false;
		audioBlip1.mute = false;
		audioBlip2.mute = false;
		audioBlip3.mute = false;
	}

	// This is my hack for beeps with different pitches because flash does not support AudioSource.pitch :(
	public static void PlayBlip()				// Text_Typewriter calls this during typing effect
	{
		int r = Random.Range (0,5);
		float vol = Random.Range (0.0f, 90.0f);

		audioBlip0.Stop();
		audioBlip1.Stop();
		audioBlip2.Stop();
		audioBlip3.Stop();

		if (r == 0)
		{
			audioBlip0.volume = vol;
			audioBlip0.Play();
		}
		if (r == 1)
		{
			audioBlip1.volume = vol;
			audioBlip1.Play();
		}
		if (r == 2)
		{
			audioBlip2.volume = vol;
			audioBlip2.Play();
		}
		if (r == 3)
		{
			audioBlip3.volume = vol;
			audioBlip3.Play();
		}
	}

	void OnReset()
	{
		currentSpawnChipCount = totalSpawnChipCount;
	}

	void OnLevelComplete()
	{
		if (currentLevel == 0)						// If we are on the menu we check if there is a save
		{
			LoadSavedLevel();
		}
		else
		{
			currentLevel ++;
			if (currentLevel < 7) PlayerPrefs.SetInt("Current Level", currentLevel);			// TEMP LEVEL LIMIT BECAUSE OF SAVE BUG OTHERWISE
		}
		// set the chipsNeeded and totalSpawnChipCount for the next level
		currentChipsNeededCount = chipsNeededPerLevel [currentLevel];
		totalSpawnChipCount = totalSpawnsPerLevel [currentLevel];
		currentSpawnChipCount = totalSpawnChipCount;
	}
}
