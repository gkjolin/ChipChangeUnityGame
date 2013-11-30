using UnityEngine;
using System.Collections;

public class Chip_Main : MonoBehaviour {
	
	public float moveSpeed = 25f;
	public float maxXVelocity = 5f;
	public float maxAngularVelocity = 3f;
	public float flipSpeed = 180f;
	public Vector2 flipYForce = new Vector2(0f,10f);

	public Transform particleDeath;
	public LayerMask checkLayerMask;	// ground, obstacle and chip layers 
	
	Rigidbody2D rb2D;
	BoxCollider2D boxCol2D;
	GameObjectPool chipPool;
	Transform thisTransform;
	Transform colTransform;
	Vector2 bottomLCStartPos;
	Vector2 sideLeftLCStartPos;
	Vector2 sideRightLCStartPos;
	Vector2 topLCStartPos;
	Vector2 bottomLCStopPos;
	Vector2 sideLeftLCStopPos;
	Vector2 sideRightLCStopPos;
	Vector2 topLCStopPos;
	RaycastHit2D rayHit2D;
	int lineCastHits;
	bool isReady;
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
		boxCol2D = GetComponent<BoxCollider2D>();
		thisTransform = transform;
		chipPool = GameObjectPool.GetPool("Chip_Pool");		//Setup the pool for spawning chips	
		Invoke("SetIsReady",1f);
	}
	
	void SetIsReady()
	{
		// Setup the positions of all the linecast checks to see if the chip is grounded or flipped over
		bottomLCStartPos = new Vector2 (boxCol2D.center.x, (boxCol2D.center.y - (boxCol2D.size.y / 2)) - 0.05f);
		bottomLCStopPos = new Vector2 (bottomLCStartPos.x, bottomLCStartPos.y - 0.5f);
		topLCStartPos = new Vector2 (boxCol2D.center.x, (boxCol2D.center.y + (boxCol2D.size.y / 2)) + 0.05f);
		topLCStopPos = new Vector2(topLCStartPos.x, topLCStartPos.y + 0.5f);
		sideLeftLCStartPos = new Vector2((boxCol2D.center.x - (boxCol2D.size.x / 2)) - 0.05f, boxCol2D.center.y);
		sideLeftLCStopPos = new Vector2(sideLeftLCStartPos.x - 0.5f, sideLeftLCStartPos.y);
		sideRightLCStartPos = new Vector2((boxCol2D.center.x + (boxCol2D.size.x / 2)) + 0.05f, boxCol2D.center.y);
		sideRightLCStopPos = new Vector2(sideRightLCStartPos.x + 0.5f, sideRightLCStartPos.y);
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

	void CheckIfFlipped()
	{


		// Use a linecast to see if Ground Layer is directly below 
		// for even better performance use LinecastNonAlloc, but keep in mind it doesnt work on Flash build

		//Debug.DrawLine(thisTransform.TransformPoint(bottomLCStartPos), bottomLCStopPos, Color.green, 2, false);

		if (Physics2D.Linecast(thisTransform.TransformPoint(bottomLCStartPos), thisTransform.TransformPoint(bottomLCStopPos), checkLayerMask))
		{
			// get the dot product of our transform compared to world transform right
			if (Vector3.Dot(Vector3.right,transform.right) > 0.85f)	// If we are roughly upright, we will say we are grounded
			{
				if (rb2D.velocity.x < maxXVelocity) Move();			// If we aren't traveling roughly full speed already, move
				return;
			}
		}

		if (!IsMovingSlowly()) return;

		// Check left
		//Debug.DrawLine(thisTransform.TransformPoint(sideLeftLCStartPos), sideLeftLCStopPos, Color.green, 2, false);
		if (Physics2D.Linecast(thisTransform.TransformPoint(sideLeftLCStartPos), thisTransform.TransformPoint(sideLeftLCStopPos), checkLayerMask))
		{
			rb2D.AddForce(flipYForce);
			rb2D.AddTorque(-flipSpeed);
			return;
		}

		//Debug.DrawLine(thisTransform.TransformPoint(sideRightLCStartPos), sideRightLCStopPos, Color.green, 2, false);
		if (Physics2D.Linecast(thisTransform.TransformPoint(sideRightLCStartPos), thisTransform.TransformPoint(sideRightLCStopPos), checkLayerMask))
		{
			rb2D.AddForce(flipYForce);
			rb2D.AddTorque(-flipSpeed);
			return;
		}
		// Check upside down
		//Debug.DrawLine(thisTransform.TransformPoint(topLCStartPos), topLCStopPos, Color.green, 2, false);
		if (Physics2D.Linecast(thisTransform.TransformPoint(topLCStartPos), thisTransform.TransformPoint(topLCStopPos), checkLayerMask))
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

	void DeathTrigger()		// Activated by Death Triggers, also called on Reset and LevelComplete
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