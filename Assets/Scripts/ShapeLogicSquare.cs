using UnityEngine;
using System.Collections;

public class ShapeLogicSquare : MonoBehaviour {
	
	public Vector2 moveSpeed = new Vector2(50f,0f);
	public Vector2 flipYForce = new Vector2(0f,50f);
	public float flipSpeed = 50f;
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
			
			if (isGrounded) rb2D.AddForce(moveSpeed);
			else if (isFlippedLeft && IsMovingSlowly())  rb2D.AddTorque(-flipSpeed);
			else if (isFlippedRight && IsMovingSlowly()) rb2D.AddTorque(flipSpeed);
			else if (isUpsideDown && IsMovingSlowly()) rb2D.AddTorque(-flipSpeed*2);
			
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