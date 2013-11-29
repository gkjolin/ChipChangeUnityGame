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
	RaycastHit2D[] lineCast = new RaycastHit2D[10];
	Collider2D thisCol2D;
	int lineCastHits;

	bool isReady;
	bool isGrounded;
	bool isClicked;
	
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
		thisCol2D = collider2D;
		thisTransform = transform;
		chipPool = GameObjectPool.GetPool("Chip_Pool");		//Setup the pool for spawning chips	
		groundCheck = transform.Find("GroundCheck");
		sideCheckLeft = transform.Find("SideCheckLeft");
		sideCheckRight = transform.Find("SideCheckRight");
		topCheck = transform.Find("TopCheck");
		Invoke("SetIsReady",1f);
	}
	
	void SetIsReady()
	{
		isReady = true;
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !isClicked)
		{
			Vector3 hitPointV3 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, 50f));
			// If it hit this gameObject change directions
			Vector3 hitDistance = hitPointV3 - transform.position;
			if(hitDistance.magnitude < 0.5f)
			{
				isClicked = true;
				Invoke("ResetIsClicked", 0.5f);
				ChangeDirections (2);
			}
		}
	}
	
	void FixedUpdate () 
	{
		if(isReady)
		{
			CheckIfFlipped();
			AngularVelocityLimitCheck();
		}
	}

	void Move()
	{
		rb2D.AddForce(moveSpeed*transform.right);
	}

	int CheckLineCast ()
	{
		int i = 0;

		while (i < lineCast.Length)
		{
			if (lineCast[i] != null)
			{ 
				if (lineCast[i].transform.collider2D != thisCol2D)
				    return i;
			}
			i++;
		}
		return -1;
	}

	void CheckIfFlipped()
	{
		// I recommend changing LineCastAll to LineCastNonAlloc to avoid garbage. Unfortunately it doesnt work on flash platform
		int lineCastInt;			// LineCastNonAlloc returns an int count of how many contacts. use it instead of linecast.Length in CheckLineCast()
		isGrounded = false;

		// Use a linecast to see if Ground Layer is directly below
		// groundCheck, sideCheck, etc are child gameobjects of this gameobject

		// so if you change to LineCastNonAlloc this next line is lineCast = Physics2D.LinecastNonAlloc(transform.position, topCheck.position,lineCast, groundAndObstacleLayerMask);
		lineCast = Physics2D.LinecastAll(transform.position, groundCheck.position, groundAndObstacleLayerMask);
		lineCastInt = CheckLineCast();
		if (lineCastInt != -1)
		{
			// get the dot product of our transform compared to world transform right
			if (Vector3.Dot(Vector3.right,transform.right) > 0.85f)	// If we are roughly upright, we will say we are grounded
			{
				isGrounded = true;
				if (rb2D.velocity.x < maxXVelocity) Move();			// If we aren't traveling roughly full speed already, move
				return;
			}
		}

		if (!IsMovingSlowly()) return;

		// Check left
		lineCast = Physics2D.LinecastAll(transform.position, sideCheckLeft.position, groundAndObstacleLayerMask);
		lineCastInt = CheckLineCast();
		if (lineCastInt != -1 )
		{
			rb2D.AddForce(flipYForce);
			rb2D.AddTorque(-flipSpeed);
			return;
		}
		// Check right
		lineCast = Physics2D.LinecastAll(transform.position, sideCheckRight.position, groundAndObstacleLayerMask);
		lineCastInt = CheckLineCast();
		if (lineCastInt != -1)
		{
			rb2D.AddForce(flipYForce);
			rb2D.AddTorque(-flipSpeed);
			return;
		}
		// Check upside down
		lineCast = Physics2D.LinecastAll(transform.position, topCheck.position, groundAndObstacleLayerMask);
		lineCastInt = CheckLineCast();
		if (lineCastInt != -1)
		{
			rb2D.AddForce(flipYForce);
			rb2D.AddTorque(-flipSpeed);
			return;
		}
	}

	void ChangeDirections(int direction)                        // Called by Trigger_TurnAround when we are switching directions
	{
		if (direction == 1)                // Always Move Right and Flip Right
		{
			Vector3 newLocalScale = new Vector3(Mathf.Abs (thisTransform.localScale.x), thisTransform.localScale.y, thisTransform.localScale.z);
			thisTransform.localScale = newLocalScale;
			moveSpeed = Mathf.Abs (moveSpeed);
			flipSpeed = Mathf.Abs (flipSpeed);
		}
		else if (direction == -1) // Always Move Left and Flip Left
		{
			Vector3 newLocalScale = new Vector3(Mathf.Abs (thisTransform.localScale.x) * -1, thisTransform.localScale.y, thisTransform.localScale.z);
			thisTransform.localScale = newLocalScale;
			moveSpeed = Mathf.Abs (moveSpeed) * -1;
			flipSpeed = Mathf.Abs (flipSpeed) * -1;
		}
		else if (direction == 2)	// Switch directions (opposite of current direction)
		{
			Vector3 newLocalScale = new Vector3(thisTransform.localScale.x * -1, thisTransform.localScale.y, thisTransform.localScale.z);
			thisTransform.localScale = newLocalScale;
			moveSpeed = moveSpeed * -1;
			flipSpeed = flipSpeed * -1;
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
		if (rb2D.velocity.x < 1f && rb2D.velocity.y < 1f && rb2D.angularVelocity < 5f) 
			return true;
		else return false;
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

	void ResetIsClicked()
	{
		isClicked = false;
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