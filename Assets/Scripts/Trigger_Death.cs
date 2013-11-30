using UnityEngine;
using System.Collections;

public class Trigger_Death : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.CompareTag("Chip"))
		{
			col.SendMessage("DeathTrigger",SendMessageOptions.DontRequireReceiver);
		}
	}
}
