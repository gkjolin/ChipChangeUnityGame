using UnityEngine;
using System.Collections;

public class Camera_Logic : MonoBehaviour {

	public Transform camPositionsParent;			// The gameObject in our scene that hold all the camera Position gameobjects

	Vector3[] camPositions;

	void OnEnable()			
	{
		Messenger.AddListener("levelComplete", OnLevelComplete);			// Register to levelComplete event		
	}
	
	void OnDisable()
	{	
		Messenger.RemoveListener("levelComplete", OnLevelComplete);			// Always make sure to unregister the event on disable
	}

	void Start()
	{
		// make an array to hold the cameraPositions. Will use to move camera to next level
		camPositions = new Vector3[camPositionsParent.childCount];
		Invoke("Setup", 0.1f);
	}

	void Setup()
	{
		StartCoroutine("SetupCamPositions");
	}

	IEnumerator SetupCamPositions()
	{
		// Run through all the children of camPositionsParent and add their position to our camPositions Vector3 array.
		for (var i=0; i < camPositions.Length; i++)
		{
			camPositions[i] = camPositionsParent.GetChild(i).transform.position;
			yield return null;
		}
		transform.position = camPositions[_Manager.currentLevel];			// Set the camera position to the current level position
	}



	void OnLevelComplete() {
		Invoke("MoveCameraToNextLevel", 0.1f);
	}

	void MoveCameraToNextLevel ()
	{
		LeanTween.move( gameObject, camPositions[_Manager.currentLevel], 
		               _Manager.camTransitionSecs, new object[]{ "delay", _Manager.camMoveDelaySecs, "ease", LeanTweenType.easeInOutSine});
	}
}
