using UnityEngine;
using System.Collections;

public class testLeanTween : MonoBehaviour {
	
	Vector3 moveTweenVector3;
	
	// Use this for initialization
	void Start () {
		moveTweenVector3 = new Vector3(transform.position.x,transform.position.y+5,transform.position.z);
		MoveTween();
	}
	
	// Update is called once per frame
	void MoveTween () {
		LeanTween.move( gameObject, moveTweenVector3, 2.0f, new object[]{ "ease",LeanTweenType.easeOutElastic,"repeat",7});
		//LeanTween.move(gameObject,moveTweenVector3,1f { "onComplete",MoveTween});
	}
}
