using UnityEngine;
using System.Collections;

public class _Manager : MonoBehaviour {

	public int levelToStart;			// These static variables can be accessed by any class via _Manager.levelToStart
	public static float camMoveDelaySecs = 3f;		// time it takes for before moving camera to next level
	public static float camTransitionSecs = 4f;		// time it takes for camera to move from level to level
	public static AudioSource audioBlip0;			// Text_Typewriter calls this during typing effect
	public static AudioSource audioBlip1;			
	public static AudioSource audioBlip2;			
	public static AudioSource audioBlip3;	

	[System.NonSerialized]
	public static int currentLevel;
	[System.NonSerialized]
	public static int score;



	void Start () 
	{
		currentLevel = levelToStart;
		AudioSource[] aSources = GetComponents<AudioSource>();
		audioBlip0 = aSources[0];
		audioBlip1 = aSources[1];
		audioBlip2 = aSources[2];
		audioBlip3 = aSources[3];
	}
	
	void OnEnable()			
	{
		Messenger.AddListener("levelComplete", OnLevelComplete);			
	}
	
	void OnDisable()
	{
		Messenger.RemoveListener("levelComplete", OnLevelComplete);			
	}

	// This is my hack for beeps with different pitches because flash does not support AudioSource.pitch :(
	public static void PlayBlip()				// Text_Typewriter calls this during typing effect
	{
		int r = Random.Range (0,5);
		audioBlip0.Stop();
		audioBlip1.Stop();
		audioBlip2.Stop();
		audioBlip3.Stop();

		if (r == 0)
		{
			audioBlip0.Play();
		}
		if (r == 1)
		{
			audioBlip1.Play();
		}
		if (r == 2)
		{
			audioBlip2.Play();
		}
		if (r == 3)
		{
			audioBlip3.Play();
		}
	}

	void OnLevelComplete()
	{
		currentLevel ++;
	}
}
