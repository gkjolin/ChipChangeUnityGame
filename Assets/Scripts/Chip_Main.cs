using UnityEngine;
using System.Collections;

public class Chip_Main : MonoBehaviour {
	
	public float moveSpeed = 25f;
	public float maxXVelocity = 5f;
	public float maxAngularVelocity = 3f;
	public float flipSpeed = 180f;
	public Vector2 flipYForce = new Vector2(0f,10f);

	public Transform particleDeath;
	public LayerMask groundAndObstacleLayerMask;
	
	Rigidbody2D rb2D;
	GameObjectPool chipPool;
	Transform thisTransform;
	Transform groundCheck;
	Transform sideCheckLeft;
	Transform sideCheckRight;
	Transform topCheck;
	RaycastHit2D groundLineCast;
	bool isReady;
	bool isGrounded;
	
	void OnEnable()			
    {
        Messenger.AddListener("reset", OnReset);			// Register to the reset event on enable
		Messenger.AddListener("levelComplete", OnLevelComplete);			
    }
	
	void OnDisable()
    {
        Messenger.RemoveListener("reset", OnReset);			// Always make sure to unregister the event on disable
		Messenger.RemoveListener("levelComplete", OnLevelComplete);			
    }
	
	void Start()
	{
		rb2D = rigidbody2D;
		thisTransform = transform;
		chipPool = GameObjectPool.GetPool("Chip_Pool");		//Setup the pool for spawning chips	
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
			AngularVelocityLimitCheck();
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
		groundLineCast = Physics2D.Linecast(transform.position, groundCheck.position, groundAndObstacleLayerMask);
		isGrounded = groundLineCast;
		
		// If grounded, move forward
		if (isGrounded && rb2D.velocity.x < maxXVelocity && groundLineCast.normal.y > .5f)
		{
			rb2D.AddForce(moveSpeed*transform.right);
		}
		// Check if square is stopped moving and flipped over, if so attempt to right itself.
		else if (IsMovingSlowly ())
		{
		
			isFlippedLeft = Physics2D.Linecast(transform.position, sideCheckLeft.position, groundAndObstacleLayerMask);
			if (isFlippedLeft)
			{
				rb2D.AddForce(flipYForce);
				rb2D.AddTorque(-flipSpeed);
			}
			else 
			{
				isFlippedRight = Physics2D.Linecast(transform.position, sideCheckRight.position, groundAndObstacleLayerMask);  
				if (isFlippedRight)
				{
					rb2D.AddForce(flipYForce);
					rb2D.AddTorque(-flipSpeed);
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
	
	void AngularVelocityLimitCheck()
	{
		if (Mathf.Abs (rb2D.angularVelocity) > maxAngularVelocity)
			rb2D.angularVelocity = Mathf.Clamp(rb2D.angularVelocity,-maxAngularVelocity,maxAngularVelocity);
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
		thisTransform.localEulerAngles = Vector3.zero;
		// Disable this gameObject
		chipPool.ReleaseInstance(thisTransform);
	}
	
	void FinishedDespawn()
	// Called by Tracker_Finish (Text_Tracker_Finish gameobject) when this chip reaches the finish
	{
		thisTransform.localEulerAngles = Vector3.zero;
		chipPool.ReleaseInstance(thisTransform);
	}
	
	void OnReset()
	{
		DeathTrigger();	
	}
	
	void OnLevelComplete()
	{
		DeathTrigger();	
	}
	
}