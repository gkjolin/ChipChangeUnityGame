using UnityEngine;
using System.Collections;

public class Obstacle_Laser : MonoBehaviour {

	public int thisLevel;
	public bool isFlashing;
	public float enabledSecs = 3.0f;
	public float disabledSecs = 3.0f;

	ParticleSystem particle;
	Collider2D col2D;
	bool isActivated;

	void OnEnable()			
	{
		Messenger.AddListener("levelComplete", OnLevelComplete);
	}
	
	void OnDisable()
	{
		Messenger.RemoveListener("levelComplete", OnLevelComplete);			
	}

	void Start () 
	{
		particle = GetComponent<ParticleSystem>();
		col2D = GetComponent<Collider2D>();
		Invoke("CheckLevel", 5f);
	}

	void CheckLevel ()
	{
		if (_Manager.currentLevel == thisLevel)
		{
			isActivated = true;
			DisableLaserRepeat();
		}

		else 
		{
			DisableLaser();
		}
	}

	void EnableLaser () 
	{
		if (isActivated)
		{
			col2D.enabled = true;
			particle.renderer.enabled = true;
			particle.time = 0;
			particle.Clear(true);
			particle.Play(true);
			if (isFlashing) Invoke("DisableLaserRepeat", enabledSecs);
		}
	}

	void DisableLaser()
	{
		col2D.enabled = false;
		particle.Stop();
		particle.time = 0;
		particle.Clear(true);
		particle.renderer.enabled = false;
		isActivated = false;
	}

	void DisableLaserRepeat() 
	{
		if (isActivated)
		{
			col2D.enabled = false;
			particle.Stop();
			particle.time = 0;
			particle.Clear(true);
			particle.renderer.enabled = false;
			Invoke("EnableLaser", disabledSecs);
		}
	}

	void OnLevelComplete()
	{
		Invoke("CheckLevel", 7f);
	}
}
