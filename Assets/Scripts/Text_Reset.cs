using UnityEngine;
using System.Collections;

public class Text_Reset : MonoBehaviour {
	
	Text_Typewriter textEffect;
	bool isActivated;

	void Start()
	{
		Invoke("Setup", 0.1f);
		textEffect = GetComponent<Text_Typewriter>();
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
	
	void Setup()
	{
		if (_Manager.currentLevel != 0) textEffect.ShowText("Reset");
	}

	void Update ()
	{
		if (isActivated) return;
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
			
			if(hit.collider != null && hit.collider.transform == transform)
			{
				isActivated = true;
				Invoke("ResetIsActivated",2f);
				Messenger.Invoke ("reset");
			}
		}
	}
	
	void ResetIsActivated()
	{
		isActivated = false;	
	}

	void OnReset()
	{
		textEffect.RemoveText (true);
	}
	
	void OnLevelComplete()
	{
		textEffect.RemoveText ();
		textEffect.ShowTextDelayed ("Reset", 7f);
	}
}
