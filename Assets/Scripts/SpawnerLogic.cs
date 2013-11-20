using UnityEngine;
using System.Collections;

public class SpawnerLogic : MonoBehaviour {

	public GameObject testSprite;

	void OnMouseUp()
	{
//		RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
//		if (ray.collider != null && ray.collider.transform == this.gameObject.transform)
//		{
			Instantiate(testSprite, new Vector3 (transform.position.x,transform.position.y,0f), transform.rotation);
			this.gameObject.SetActive(false);
//		}
	}
}