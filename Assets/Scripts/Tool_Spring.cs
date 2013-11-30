using UnityEngine;
using System.Collections;

public class Tool_Spring : MonoBehaviour {
	
	public float force = 2;
	public GameObject particleSplash;

	Rigidbody2D colRb2D;

	void OnCollisionEnter2D (Collision2D col) 
	{
		if (col.transform.CompareTag("Chip"))
		{

			colRb2D = col.transform.rigidbody2D;
			colRb2D.AddForce(col.relativeVelocity * force);
			Instantiate(particleSplash, col.contacts[0].point,  Quaternion.Euler(-90,0,transform.localEulerAngles.z));
		}
	}
}
