using UnityEngine;
using System.Collections;

public class Tool_Spring : MonoBehaviour {
	
	public Vector2 forceAmount = new Vector2(500f,2000f);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnCollisionEnter2D (Collision2D col) 
	{
		if (col.gameObject.CompareTag("Shape"))
		{
			Vector2 shapeNormal = col.gameObject.rigidbody2D.velocity.normalized;
			shapeNormal.y += .5f;
			col.gameObject.rigidbody2D.AddForce(forceAmount);
		}
	}
}
