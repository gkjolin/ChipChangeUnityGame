using UnityEngine;
using System.Collections;

public class _Manager : MonoBehaviour {

	public int levelToStart;			// These static variables can be accessed by any class via _Manager.levelToStart
	public static float camMoveDelaySecs = 3f;		// time it takes for before moving camera to next level
	public static float camTransitionSecs = 4f;		// time it takes for camera to move from level to level

	[System.NonSerialized]
	public static int currentLevel;
	[System.NonSerialized]
	public static int score;

	void Start () {
		currentLevel = levelToStart;
	}

	void OnEnable()			
	{
		Messenger.AddListener("levelComplete", OnLevelComplete);			
	}
	
	void OnDisable()
	{
		Messenger.RemoveListener("levelComplete", OnLevelComplete);			
	}

	void OnLevelComplete()
	{
		currentLevel ++;
	}
}
