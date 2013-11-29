using UnityEngine;
using System.Collections;

public class Tool_Bridge_Trigger: MonoBehaviour {
	
	bool isTriggered;
	Rigidbody2D parentRb2D;
	Transform parentTransform;
	
	void Start()
	{
		parentTransform = transform.parent;
		parentRb2D = transform.parent.gameObject.rigidbody2D;
	}
	
	void OnTriggerStay2D (Collider2D col) {
		if(!isTriggered)
		{
			isTriggered = true;
			parentRb2D.AddForceAtPosition(new Vector2(-100f,0f), new Vector2(1f,5f));
			Invoke("SetKinematic",1.5f);
			
		}
	}
	
	void SetKinematic()
	{
		parentRb2D.isKinematic = true;
		Vector3 moveParentDownPos = new Vector3(parentTransform.position.x,parentTransform.position.y-.1f,parentTransform.position.z);
		LeanTween.move( parentTransform.gameObject, moveParentDownPos, .8f, new object[]{ "ease",LeanTweenType.easeInOutSine});
		//parentRb2D.gravityScale = 3f;
	}
	
	
}
