using UnityEngine;
using System.Collections;

public class Tool_Spring : MonoBehaviour {

	public float force = 20f;
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
		if (isActivated) return;
		if (col.gameObject.layer == 13 && !isActivated)
		{
			isActivated = true;
			Invoke("ResetIsActivated", 0.25f);
			_Manager.audioSpring.Play();
			colTrans = col.transform;
			// This objs parent has the rotation info. Set the chip's velocity to match the angle of this spring * force
			colTrans.rigidbody2D.angularVelocity = 0f;
			Vector2 forceAngle = new Vector2(transform.parent.up.x*3f,transform.parent.up.y*0.4f);

			colTrans.rigidbody2D.velocity = (force * forceAngle);
			// Spawn a particle effect at the contact point.
			particle = particlePool.GetInstance(col.contacts[0].point);
			particle.rotation = Quaternion.Euler(-90,0,transform.parent.localEulerAngles.z);
		}
	}

	void ResetIsActivated()
	{
		isActivated = false;
	}
}
