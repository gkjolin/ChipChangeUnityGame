using UnityEngine;
using System.Collections;

public class Obstacle_Laser : MonoBehaviour {

	public float enabledSecs = 3.0f;
	public float disabledSecs = 3.0f;

	ParticleSystem particle;
	Collider2D col2D;

	void Start () 
	{
		particle = GetComponent<ParticleSystem>();
		col2D = GetComponent<Collider2D>();
		Invoke("DisableLaser",enabledSecs);
	}

	void EnableLaser () 
	{
		col2D.enabled = true;
		particle.renderer.enabled = true;
		particle.time = 0;
		particle.Clear(true);
		particle.Play(true);
		Invoke("DisableLaser", enabledSecs);
	}

	void DisableLaser() 
	{
		col2D.enabled = false;
		particle.Stop();
		particle.time = 0;
		particle.Clear(true);
		particle.renderer.enabled = false;
		Invoke("EnableLaser", disabledSecs);
	}
}
