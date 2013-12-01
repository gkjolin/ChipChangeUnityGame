using UnityEngine;
using System.Collections;

public class Obstacle_Smasher : MonoBehaviour {

	public int thisLevel;

	Vector3 moveTweenVector3;

	void OnEnable()			
	{
		Messenger.AddListener("levelComplete", OnLevelComplete);
		Invoke("MoveTween", 5f);		// Start the tween if the game starts on this level
	}
	
	void OnDisable()
	{
		Messenger.RemoveListener("levelComplete", OnLevelComplete);			
	}

	void Start () 
	{
		moveTweenVector3 = new Vector3(transform.position.x,transform.position.y+3,transform.position.z);
	}

	void MoveTween () 
	{
		if(_Manager.currentLevel == thisLevel)		// If we are on thisLevel, start the tween
		{
			LeanTween.move( gameObject, moveTweenVector3, 1f, new object[]{ "ease",LeanTweenType.easeOutElastic,"repeat",-1});
		}
		else
		{
			LeanTween.cancel (gameObject);	// Cancel the tween if it exists.
		}
	}

	void OnLevelComplete()
	{
		Invoke("MoveTween", 8f);
	}
}
