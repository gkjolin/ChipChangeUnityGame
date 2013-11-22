using UnityEngine;
using System.Collections;

public class ShapeLogicSquare : MonoBehaviour {
	
	public Vector2 moveSpeed = new Vector2(50f,0f);
	public Vector2 flipYForce = new Vector2(0f,50f);
	public float flipSpeed = 60f;
	public LayerMask groundAndObstacleLayerMask;
	
	private Rigidbody2D rb2D;
	private Transform groundCheck;
	private Transform sideCheckLeft;
	private Transform sideCheckRight;
	private Transform topCheck;
	
	private bool isReady;
	private bool isGrounded;
	private bool isFlippedLeft;
	private bool isFlippedRight;
	private bool isUpsideDown;
	
	void Start()
	{
		rb2D = rigidbody2D;
		groundCheck = transform.Find("GroundCheck");
		sideCheckLeft = transform.Find("SideCheckLeft");
		sideCheckRight = transform.Find("SideCheckRight");
		topCheck = transform.Find("TopCheck");
		
		Invoke("SetIsReady",1f);
	}
	
	void FixedUpdate () 
	{
		if(isReady)
		{
			isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, groundAndObstacleLayerMask);
			isFlippedLeft = Physics2D.Linecast(transform.position, sideCheckLeft.position, groundAndObstacleLayerMask);  
			isFlippedRight = Physics2D.Linecast(transform.position, sideCheckRight.position, groundAndObstacleLayerMask);  
			isUpsideDown = Physics2D.Linecast(transform.position, topCheck.position, groundAndObstacleLayerMask);  
			
			bool isMovingSlowly = IsMovingSlowly ();
			if (isGrounded)
			{
				rb2D.AddForce(moveSpeed);
			}
			else if (isFlippedLeft && isMovingSlowly)
			{
				rb2D.AddForce(flipYForce);
				rb2D.AddTorque(-flipSpeed);
			}
			else if (isFlippedRight && isMovingSlowly)
			{
				rb2D.AddForce(flipYForce);
				rb2D.AddTorque(flipSpeed);
			}
			else if (isUpsideDown && isMovingSlowly)
			{
				rb2D.AddForce(flipYForce);
				rb2D.AddTorque(-flipSpeed);
			}
			
			// Have to help flip over by adding some y force
			//if(isFlippedLeft) rb2D.AddForce(flipYForce);
			
			isGrounded = false;
			isFlippedLeft = false;
			isFlippedRight = false;
			isUpsideDown = false;
		}
	}
	
	void SetIsReady()
	{
		isReady = true;
	}
	
	bool IsMovingSlowly()
	{
		if (rb2D.velocity.x < 1 && rb2D.velocity.y < 1) return true;
		else return false;
	}
	
}