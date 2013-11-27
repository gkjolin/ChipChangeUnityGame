using UnityEngine;
using System.Collections;

public class Text_Typewriter : MonoBehaviour {

	public string text;					// The text to show
	public float startDelaySecs;		// How long in seconds till the effect starts
	
	TextMesh textMesh;					
	int currentChar;					// A counter for which character we just displayed
	
	void Start ()
	{
		currentChar = text.Length;
		textMesh = GetComponent<TextMesh>();
		InvokeRepeating ("ShowText",startDelaySecs+0.01f,0.1f);	// We will use InvokeRepeating to run ShowText multiple times
	}
	
	void ShowText () 
	{
		if (currentChar <= 0)
		{
			CancelInvoke("ShowText");
			return;
		}
		textMesh.text = textMesh.text + text[text.Length-currentChar];
		currentChar -= 1;
	}
}
