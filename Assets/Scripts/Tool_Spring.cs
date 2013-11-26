using UnityEngine;
using System.Collections;

public class Tool_Spring : MonoBehaviour {
	
	public Vector2 forceAmount = new Vector2(500f,2000f);
	
	private Transform sprite;
		
	void Start () {
		sprite = transform.Find("Particle");
	}
	
	void OnCollisionEnter2D (Collision2D col) 
	{
		if (col.gameObject.CompareTag("Shape"))
		{
			Vector2 shapeNormal = col.gameObject.rigidbody2D.velocity.normalized;
			shapeNormal.y += .5f;
			col.gameObject.rigidbody2D.AddForce(forceAmount);
			
			LeanTween.scale( sprite.gameObject, sprite.localScale * 1.1f, .1f, new object[]{ "ease",LeanTweenType.easeOutSine,"repeat",2,"loopType",LeanTweenType.pingPong });

		}
	}
}
