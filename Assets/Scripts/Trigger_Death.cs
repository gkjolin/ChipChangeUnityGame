using UnityEngine;
using System.Collections;

public class Trigger_Death : MonoBehaviour {
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.CompareTag("Chip"))
		{
			col.SendMessage("DeathTrigger",SendMessageOptions.DontRequireReceiver);
		}
	}
}
