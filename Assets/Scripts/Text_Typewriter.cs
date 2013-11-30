using UnityEngine;
using System.Collections;
using System.Text;

public class Text_Typewriter : MonoBehaviour {

	public float startDelaySecs;		// How long in seconds till the effect starts
	public bool removeOnReset;
	public bool removeOnLevelComplete;
	public bool showTextOnStart;
	public string textToShowOnStart;
		
	public string TextToShow {  get; set;}

	const float ShowTextSpeed = 0.75f;
	bool showOnRemoveComplete;
	StringBuilder textBuilder = new StringBuilder();			// Will use a stringbuilder instead of concatenating strings to reduce garbage
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
		textMesh = GetComponent<TextMesh>();
		if (showTextOnStart) Invoke("ShowTextStart", 2f);
	}

	void ShowTextStart()
	{
		if (renderer.isVisible) ShowTextDelayed(textToShowOnStart, startDelaySecs);				// If this gameobject is visible by a camera, show it
	}

	public void ShowText ()									// Show text no params will use TextToShow property with 0 seconds delay
	{
		ShowText (TextToShow);
	}

	public void ShowText (string text)
	{
		textBuilder.Length = 0;
		TextToShow = text;						
		currentChar = TextToShow.Length;
		InvokeRepeating ("ShowTextRepeater",0.01f,0.075f);
	}

	public void ShowTextDelayed (string text, float delaySecs)
	{
		StartCoroutine (ShowTextDelayedCoroutine (text, delaySecs));		// Start a coroutine so we can stall ShowText call for delaySecs 		
	}

	IEnumerator ShowTextDelayedCoroutine(string text, float delaySecs)
	{
		yield return new WaitForSeconds (delaySecs);
		ShowText (text);
	}


	
	void ShowTextRepeater () 
	{
		// if currentChar is 0, we've added all our text to the screen. cancel and return.
		if (currentChar <= 0)
		{
			CancelInvoke("ShowTextRepeater");
			return;
		}
		// add the next character to the string builder and update the textmesh
		textBuilder.Append(TextToShow[TextToShow.Length-currentChar]);
		textMesh.text = textBuilder.ToString();
		_Manager.PlayBlip();
		currentChar -= 1;
	}

	public void RemoveText ()			// multiple RemoveText methods so the bool parameter is optional
	{
		RemoveText(false);
	}

	public void RemoveText (float delaySecs)
	{
		Invoke("RemoveText", delaySecs);
	}

	public void RemoveText (bool showOnComplete)
	{
		CancelInvoke("ShowTextRepeater");
		showOnRemoveComplete = showOnComplete;
		if (textMesh.text.Length == 0) return;			// No text is showing, escape
		currentChar = TextToShow.Length;
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
				ShowText(TextToShow);
			}
			return;
		}
		// remove the last character to the string builder and update the textmesh
		textBuilder.Remove(textBuilder.Length-1,1);
		textMesh.text = textBuilder.ToString();
		currentChar += 1;
	}
	
	void OnReset()
	{
		if (removeOnReset)
		{
			CancelInvoke("ShowTextRepeater");
			RemoveText (true);
		}
	}
	
	void OnLevelComplete()
	{
		if (removeOnLevelComplete) RemoveText ();
		if (showTextOnStart) Invoke("ShowTextStart", 8f);		// Call the showText again for instruction text on the next level

	}
}
