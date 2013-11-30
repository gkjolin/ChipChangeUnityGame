using UnityEngine;
using System.Collections;

public class Tool_Spring : MonoBehaviour {

	public float force = 1200f;
	public GameObject particleSplash;

	Transform particle;
	Transform colTrans;
	GameObjectPool particlePool;
	bool isActivated;

	void Start()
	{
		particlePool = GameObjectPool.GetPool("SpringParticle_Pool");
	}

	void OnCollisionEnter2D (Collision2D col) 
	{
		print("enter");
		if (isActivated) return;
		if (col.gameObject.layer == 13)
		{
			isActivated = true;
			Invoke("ResetIsActivated", 0.1f);

			colTrans = col.transform;
			Vector2 colTransUp = col.transform.up;
			colTrans.rigidbody2D.velocity = (force * transform.parent.up);
	//		colTrans.rigidbody2D.AddForce(force * colTransUp);
			print(force * transform.parent.up);
			particle = particlePool.GetInstance(col.contacts[0].point);
			particle.rotation = Quaternion.Euler(-90,0,transform.localEulerAngles.z);
		}
	}

	void ResetIsActivated()
	{
		isActivated = false;
	}

	void OnCollisionStay2D (Collision2D col) 
	{
		print("stay");
		if (isActivated) return;
		if (col.gameObject.layer == 13)
		{
			isActivated = true;
			Invoke("ResetIsActivated", 0.1f);
			
			colTrans = col.transform;
			Vector2 colTransUp = col.transform.up;
			colTrans.rigidbody2D.AddForce(force * colTransUp);
			
			particle = particlePool.GetInstance(col.contacts[0].point);
			particle.rotation = Quaternion.Euler(-90,0,transform.localEulerAngles.z);
		}
	}
}
