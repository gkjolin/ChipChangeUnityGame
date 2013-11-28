﻿using UnityEngine;
using System.Collections;
using System.Text;

public class Text_Typewriter : MonoBehaviour {

	public float startDelaySecs;		// How long in seconds till the effect starts
	public bool removeOnReset;
	public bool removeOnLevelComplete = true;
	public bool showTextOnStart;
	public string textToShowOnStart;
		
	public string TextToShow {  get; set;}
	private string textToShow;
	
	private const float ShowTextSpeed = 0.75f;
	private bool showOnRemoveComplete;
	StringBuilder textBuilder;			// Will use a stringbuilder instead of concatenating strings to reduce garbage
	TextMesh textMesh;					
	int currentChar;					// A counter for which character we just displayed
	
	void OnEnable()			
    {
        Messenger.AddListener("reset", OnReset);			// Register to the reset event on enable
		Messenger.AddListener("levelComplete", OnLevelComplete);			
    }
	
	void OnDisable()
    {
        Messenger.RemoveListener("reset", OnReset);			// Always make sure to unregister the event on disable
		Messenger.RemoveListener("levelComplete", OnLevelComplete);			
    }
	
	void Start ()
	{
		textBuilder = new StringBuilder();
		textMesh = GetComponent<TextMesh>();
		if (showTextOnStart) ShowText(textToShowOnStart);
	}
	
	public void ShowText (string text)
	{
		textBuilder.Length = 0;
		textToShow = text;
		currentChar = textToShow.Length;
		InvokeRepeating ("ShowTextRepeater",startDelaySecs+0.01f,0.075f);
	}
	
	void ShowTextRepeater () 
	{
		// if currentChar is 0, we've added all our text to the screen. cancel and return.
		if (currentChar <= 0)
		{
			CancelInvoke("ShowText");
			return;
		}
		// add the next character to the string builder and update the textmesh
		textBuilder.Append(textToShow[textToShow.Length-currentChar]);
		textMesh.text = textBuilder.ToString();
		currentChar -= 1;
	}
	
	public void RemoveText (bool showOnComplete)
	{
		print(textBuilder.Length);
		showOnRemoveComplete = showOnComplete;
		if (textMesh.text.Length == 0) return;			// No text is showing, escape
		currentChar = 0;
		InvokeRepeating ("RemoveTextRepeater",0.01f,0.05f);
	}
	
	void RemoveTextRepeater ()
	{
		// if textBuilder length is 0, we've removed all our text from the screen. cancel and return.
		if (textBuilder.Length == 0)
		{
			CancelInvoke("RemoveTextRepeater");
			if (showOnRemoveComplete)
			{
				currentChar = 0;
				ShowText(textToShow);
			}
			return;
		}
		print("count");
		// remove the last character to the string builder and update the textmesh
		textBuilder.Remove(textBuilder.Length-1,1);
		textMesh.text = textBuilder.ToString();
		currentChar += 1;
	}
	
	void OnReset()
	{
		CancelInvoke();
		if (removeOnReset) RemoveText (true);
	}
	
	void OnLevelComplete()
	{
		if (removeOnLevelComplete) RemoveText (true);
	}
}
