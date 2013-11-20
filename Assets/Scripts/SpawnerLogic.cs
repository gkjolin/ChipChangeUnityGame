using UnityEngine;
using System.Collections;

public class SpawnerLogic : MonoBehaviour {

	public GameObject testSprite;
	private bool isReady;
	private bool isActivated;
	Transform thisTransform;
	Vector3 point;
	
	void Start(){
		thisTransform = transform;
		Invoke("SetIsReady",3f);
	}
	
	void SetIsReady()
	{
		isReady = true;
	}
	
	void OnMouseUp()
	{
		if(isReady && !isActivated)
		{
			isActivated = true;
			
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			point = ray.origin + (ray.direction * -Camera.main.gameObject.transform.position.z);
		    point = new Vector3(point.x,point.y,0f);
			LeanTween.move( gameObject, point, .7f, new object[]{ "ease",LeanTweenType.easeOutQuad});
			Invoke("SpawnShapeAndDisableThis",.75f);
		}
	}
	
	void SpawnShapeAndDisableThis()
	{
		Instantiate(testSprite, point, transform.rotation);
		this.gameObject.SetActive(false);
	}
}