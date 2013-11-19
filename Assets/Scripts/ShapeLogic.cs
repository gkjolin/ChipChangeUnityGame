using UnityEngine;
using System.Collections;

public class ShapeLogic : MonoBehaviour {
	
	public Vector2 moveSpeed = new Vector2(50f,0f);

	void Start () {
	
	}
	
	void FixedUpdate () {
		rigidbody2D.AddForce(moveSpeed);
	}
	
}