using UnityEngine;
using System.Collections;

public class Reset_Position : MonoBehaviour {

	Vector3 originalPosition;
	Quaternion originalRotation;
	Rigidbody2D rb2D;					// cache rigidbody2d component for performance reasons

	void Start ()
	{
		if (rigidbody2D != null) rb2D = rigidbody2D;
		originalPosition = transform.position;
	}

	void OnEnable()			
	{
		Messenger.AddListener("reset", OnReset);			// Register to the reset event on enable
	}
	
	void OnDisable()
	{
		Messenger.RemoveListener("reset", OnReset);			// Always make sure to unregister the event on disable
	}

	void OnReset ()
	{
		Invoke("ResetThis", 0.2f);			// Slight delay to let any chips be destroyed before repositioning this object
	}

	void ResetThis ()
	{
		if (rb2D != null){
			rb2D.velocity = Vector2.zero;
			rb2D.angularVelocity = 0f;
		}
		transform.rotation = originalRotation;
		transform.position = originalPosition;
	}
}
