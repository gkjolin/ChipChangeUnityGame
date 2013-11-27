using UnityEngine;
using System.Collections;

public class Chip_Main : MonoBehaviour {
	
	public Vector2 moveYForce = new Vector2(0f, 5f);
	public Vector2 moveYForcePos = new Vector2(1f,-.5f);
	public Vector2 flipYForce = new Vector2(0f,50f);
	public float moveSpeed = 30f;
	public float maxXVelocity = 50f;
	public float flipSpeed = 60f;
	public Transform particleFinish; 
	public Transform particleDeath;
	public LayerMask groundAndObstacleLayerMask;
	
	private Rigidbody2D rb2D;
	private Transform thisTransform;
	private Transform groundCheck;
	private Transform sideCheckLeft;
	private Transform sideCheckRight;
	private Transform topCheck;
	
	private bool isReady;
	private bool isGrounded;
	
	void OnEnable()			
    {
        Messenger.AddListener("reset", OnReset);			// Register to the reset event on enable
    }
	
	void OnDisable()
    {
        Messenger.RemoveListener("reset", OnReset);			// Always make sure to unregister the event on disable
    }
	
	void Start()
	{
		rb2D = rigidbody2D;
		thisTransform = transform;
		groundCheck = transform.Find("GroundCheck");
		sideCheckLeft = transform.Find("SideCheckLeft");
		sideCheckRight = transform.Find("SideCheckRight");
		topCheck = transform.Find("TopCheck");
		
		Invoke("StartMoving",1f);
	}
	
	void StartMoving()
	{
		isReady = true;
	}
	
	void StopMoving()
	{
		isReady = false;
	}
	
	void StopMovingDelayed(float time)
	{
		Invoke("StopMoving", time);
	}
	
	void FixedUpdate () 
	{
		if(isReady)
		{
			MoveAndCheckIfFlipped();
		}
	}
	
	void MoveAndCheckIfFlipped()
	{
		isGrounded = false;
		bool isFlippedLeft = false;
		bool isFlippedRight = false;
		bool isUpsideDown = false;
		
		// Use a linecast to see if Ground Layer is directly below
		// groundCheck, sideCheck, etc are child gameobjects of this gameobject
		isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, groundAndObstacleLayerMask);
			
		// If grounded, move forward
		if (isGrounded && rb2D.velocity.x < maxXVelocity)
		{
			rb2D.AddForce(moveSpeed*transform.right);
			//rb2D.AddForceAtPosition(moveYForce,moveYForcePos);
		}
		// Check if square is stopped moving and flipped over, if so attempt to right itself.
		else if (IsMovingSlowly ())
		{
		
			isFlippedLeft = Physics2D.Linecast(transform.position, sideCheckLeft.position, groundAndObstacleLayerMask);
			if (isFlippedLeft)
			{
				//rb2D.AddForce(flipYForce);
				rb2D.AddTorque(-flipSpeed);
			}
			else 
			{
				isFlippedRight = Physics2D.Linecast(transform.position, sideCheckRight.position, groundAndObstacleLayerMask);  
				if (isFlippedRight)
				{
					//rb2D.AddForce(flipYForce);
					rb2D.AddTorque(flipSpeed);
				}
				else 
				{
		  			isUpsideDown = Physics2D.Linecast(transform.position, topCheck.position, groundAndObstacleLayerMask);  
					if (isUpsideDown)
					{
						rb2D.AddForce(flipYForce);
						rb2D.AddTorque(-flipSpeed);
					}
				}
			}
		}	
	}

	
	bool IsMovingSlowly()
	{
		// Checks if velocity and angularVelocity are low
		if (rb2D.velocity.x < 1f && rb2D.velocity.y < 1f
			&& rb2D.angularVelocity < 5f) 
				return true;
		else return false;
	}

	
	void DeathTrigger()		// Activated by Death Triggers
	{
		// Instantiate death particle prefab
		Instantiate ( particleDeath, thisTransform.position, Quaternion.identity);
		
		// Disable this gameObject
		gameObject.SetActive(false);
	}
	
	void FinishedDespawn()
	// Called by Tracker_Finish (Text_Tracker_Finish gameobject) when this chip reaches the finish
	{
		Instantiate(particleFinish, new Vector3(thisTransform.position.x,thisTransform.position.y,-10f)
			,Quaternion.Euler(new Vector3(0f,90f,270f)));
		gameObject.SetActive(false);
	}
	
	void OnReset()
	{
		// do somethin	
	}
	
}