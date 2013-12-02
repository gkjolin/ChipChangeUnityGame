using UnityEngine;
using System.Collections;

public class Text_Restart : MonoBehaviour {

	bool isActivated;	
	float oldCamMoveDelaySecs;
	float oldCamTransitionSecs;

	void Start()
	{
		if (PlayerPrefs.GetInt("Current Level") <= 1)		// If the player has previously gotten to level 2 we will show this
		{
			gameObject.SetActive (false);
		}
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
				PlayerPrefs.SetInt("Current Level", 0);					// Reset the Current Level int saved to playerprefs	

				oldCamMoveDelaySecs = _Manager.camMoveDelaySecs;			// Altering the settings of Camera transitions for the title screen
				oldCamTransitionSecs = _Manager.camTransitionSecs;
				_Manager.camMoveDelaySecs = 1.5f;
				_Manager.camTransitionSecs = 5.5f;

				Messenger.Invoke ("levelComplete");
				Invoke("DisableThis",4f);
			}
		}
	}
	
	void DisableThis()
	{
		_Manager.camMoveDelaySecs = oldCamMoveDelaySecs;	// resetting camera transition settings
		_Manager.camTransitionSecs = oldCamTransitionSecs;
		gameObject.SetActive (false);	
	}
}
