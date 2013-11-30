using UnityEngine;
using System.Collections;

public class Tool_Spring : MonoBehaviour {
	
	public Vector2 forceAmount = new Vector2(500f,2000f);
	public GameObject particleSplash;
	
	void OnCollisionEnter2D (Collision2D col) 
	{
		if (col.gameObject.CompareTag("Chip"))
		{
			Vector2 shapeNormal = col.gameObject.rigidbody2D.velocity.normalized;
			shapeNormal.y += .5f;
			col.gameObject.rigidbody2D.AddForce(forceAmount);
			
			Instantiate(particleSplash, col.contacts[0].point,  Quaternion.Euler(-90,0,transform.localEulerAngles.z));

		}
	}
}
