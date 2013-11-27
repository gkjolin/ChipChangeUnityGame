﻿using UnityEngine;
using System.Collections;

public class Death_Trigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.gameObject.CompareTag("Chip"))
		{
			col.gameObject.SendMessage("DeathTrigger",SendMessageOptions.DontRequireReceiver);
		}
	}
}
