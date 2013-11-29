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
	Transform colTransform;
	RaycastHit2D groundLineCast;
	bool isReady;
	bool isGrounded;
	bool hitOtherChip;
	
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

	void ChangeDirections()
	{
		Vector3 newLocalScale = new Vector3(thisTransform.localScale.x * -1, thisTransform.localScale.y, thisTransform.localScale.z);
		thisTransform.localScale = newLocalScale;
		moveSpeed = moveSpeed * -1;
		flipSpeed = flipSpeed * -1;
	}

	void ChangeDirections(int direction)			// Called by Trigger_TurnAround when we are switching directions
	{
		if (direction == 1)		// Move Right and Flip Right
		{
			Vector3 newLocalScale = new Vector3(Mathf.Abs (thisTransform.localScale.x), thisTransform.localScale.y, thisTransform.localScale.z);
			thisTransform.localScale = newLocalScale;
			moveSpeed = Mathf.Abs (moveSpeed);
			flipSpeed = Mathf.Abs (flipSpeed);
		}
		else if (direction == -1) // Move Left and Flip Left
		{
			Vector3 newLocalScale = new Vector3(Mathf.Abs (thisTransform.localScale.x) * -1, thisTransform.localScale.y, thisTransform.localScale.z);
			thisTransform.localScale = newLocalScale;
			moveSpeed = Mathf.Abs (moveSpeed) * -1;
			flipSpeed = Mathf.Abs (flipSpeed) * -1;
		}
	}
	
	void DeathTrigger()		// Activated by Death Triggers
	{
		// Instantiate death particle prefab
		Instantiate ( particleDeath, thisTransform.position, Quaternion.identity);
		thisTransform.localEulerAngles = Vector3.zero;
		ChangeDirections(1);
		// Disable this gameObject
		chipPool.ReleaseInstance(thisTransform);
	}
	
	void FinishedDespawn()
	// Called by Tracker_Finish (Text_Tracker_Finish gameobject) when this chip reaches the finish
	{
		thisTransform.localEulerAngles = Vector3.zero;
		chipPool.ReleaseInstance(thisTransform);
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.layer == 13 && !hitOtherChip)
		{
			colTransform = col.gameObject.transform;
			if (colTransform.localScale.x > 0 && thisTransform.localScale.x < 0
			    || colTransform.localScale.x < 0 && thisTransform.localScale.x > 0)
				ChangeDirections();
		}
	}

	void ResetHitOtherChip()
	{
		hitOtherChip = false;
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