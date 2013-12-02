using UnityEngine;
using System.Collections;

public class Text_Mute : MonoBehaviour {

	public AudioSource music;

	Text_Typewriter textEffect;
	bool isActivated;

	void Start()
	{
		textEffect = GetComponent<Text_Typewriter>();
		Invoke ("ShowText", 3f);
	}

	void ShowText()
	{
		textEffect.ShowText ("mute");
	}

	void Update()
	{
		if (isActivated) return;
		if (Input.GetMouseButtonDown(0))
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);
			
			if(hit.collider != null && hit.collider.transform == transform)
			{
				isActivated = true;
				if (music.mute)
				{
					music.mute = false;
					_Manager.UnMute();
					textEffect.RemoveText ();
					textEffect.ShowTextDelayed ("mute", 1f);
				}
				else
				{
					music.mute = true;
					_Manager.Mute();
					textEffect.RemoveText ();
					textEffect.ShowTextDelayed ("unmute", 1f);
				}
				Invoke("ResetIsActivated", 0.5f);
			}
		}
	}

	void ResetIsActivated()
	{
		isActivated = false;	
	}
}
