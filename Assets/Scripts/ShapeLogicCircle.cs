using UnityEngine;
using System.Collections;

public class ShapeLogicCircle : MonoBehaviour {
	
	
	public Vector2 moveSpeed = new Vector2(5f,0f);
	public Vector2 bounceHeight = new Vector2(0f,20f);
	
	private Transform thisTransform;
	private Rigidbody2D rb2D;
	private int groundLayer = 9;
	private int obstacleLayer = 10;
	private bool hasBounced;
	private bool hasChangedDirection;
	private bool isReady;
	private bool isAddingForceRight;
	void Start()
	{
		rb2D = rigidbody2D;
		thisTransform = transform;
		isReady = true;
	}
	
	void FixedUpdate () {
		if(isReady)
		{
			// Change the direction of moveSpeed.x if gameObject 
			// velocity.x is moving the other direction
			if (moveSpeed.x > 0 && rb2D.velocity.x < -5f)
				moveSpeed = new Vector2 (-moveSpeed.x,moveSpeed.y);
			else if (moveSpeed.x < 0 && rb2D.velocity.x > 5f)
				moveSpeed = new Vector2 (-moveSpeed.x,moveSpeed.y);
			
			rb2D.AddForce(moveSpeed);
		}
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.layer == groundLayer)
		{
			if(!hasBounced)
			{
				hasBounced = true;
				float maxBounceYVelocity = 10f;
				if(Mathf.Abs (rb2D.velocity.y) < maxBounceYVelocity);
					rb2D.AddForce(bounceHeight);
				Invoke("ResetHasBounced",2f);
			}
		}
	}
	
	void OnCollisionStay2D(Collision2D col)
	{
		if(col.gameObject.layer == groundLayer)
		{
			if(!hasBounced)
			{
				hasBounced = true;
				rb2D.AddForceAtPosition(bounceHeight,new Vector2(thisTransform.position.x, thisTransform.position.y-1));
				print("groound");
				Invoke("ResetHasBounced",1f);
			}
		}
	}
	
	void ResetHasBounced()
	{
		CancelInvoke("ResetHasBounced");
		hasBounced = false;
	}
	
	void ResetHasChangedDirection()
	{
		CancelInvoke("ResetHasChangedDirection");
		hasChangedDirection = false;
	}
}