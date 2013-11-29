using UnityEngine;
using System.Collections;

public class Trigger_TurnAround : MonoBehaviour {

	public bool turnLeft;
	public bool turnRight;

	bool isActivated;
	Transform colTransform;

//	void OnTriggerEnter2D (Collider2D col)
//	{
//		if (isActivated) return;
//		if (col.gameObject.layer == 13)
//		{
//			print("enter");
//			isActivated = true;
//			Invoke("ResetIsActivated", 0.1f);
//			colTransform = col.gameObject.transform;
//			if (turnLeft) colTransform.SendMessage("ChangeDirections", -1);
//			else if (turnRight) colTransform.SendMessage("ChangeDirections", 1);
//
//		}
//	}

	void OnTriggerStay2D (Collider2D col)
	{
		if (isActivated) return;
		if (col.gameObject.layer == 13)
		{
			isActivated = true;
			Invoke("ResetIsActivated", 0.1f);
			colTransform = col.gameObject.transform;
			if (turnLeft) colTransform.SendMessage("ChangeDirections", -1);
			else if (turnRight) colTransform.SendMessage("ChangeDirections", 1);
			
		}
	}

	void ResetIsActivated()
	{
		isActivated = false;
	}
}
