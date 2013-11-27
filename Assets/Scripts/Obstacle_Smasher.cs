using UnityEngine;
using System.Collections;

public class Obstacle_Smasher : MonoBehaviour {
	
	Vector3 moveTweenVector3;
	
	// Use this for initialization
	void Start () {
		
		moveTweenVector3 = new Vector3(transform.position.x,transform.position.y+3,transform.position.z);
		MoveTween();
	}
	
	// Update is called once per frame
	void MoveTween () {
		LeanTween.move( gameObject, moveTweenVector3, 1f, new object[]{ "ease",LeanTweenType.easeOutElastic,"repeat",-1});
		//LeanTween.move(gameObject,moveTweenVector3,1f { "onComplete",MoveTween});
	}
}
