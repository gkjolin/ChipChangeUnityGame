using UnityEngine;
using System.Collections;
using System.Text;

public class Text_Typewriter : MonoBehaviour {

	public string text;					// The text to show
	public float startDelaySecs;		// How long in seconds till the effect starts
	
	StringBuilder textBuilder;			// Will use a stringbuilder instead of concatenating strings to reduce garbage
	TextMesh textMesh;					
	int currentChar;					// A counter for which character we just displayed
	
	void OnEnable()			
    {
        Messenger.AddListener("reset", OnReset);			// Register to the reset event on enable
    }
	
	void OnDisable()
    {
        Messenger.RemoveListener("reset", OnReset);			// Always make sure to unregister the event on disable
    }
	
	void Start ()
	{
		textBuilder = new StringBuilder(text.Length);
		currentChar = text.Length;
		textMesh = GetComponent<TextMesh>();
		InvokeRepeating ("ShowText",startDelaySecs+0.01f,0.1f);	// We will use InvokeRepeating to run ShowText multiple times
	}
	
	void ShowText () 
	{
		// if currentChar is 0, we've added all our text to the screen. cancel and return.
		if (currentChar <= 0)
		{
			CancelInvoke("ShowText");
			return;
		}
		// add the next character to the string builder and update the textmesh
		textBuilder.Append(text[text.Length-currentChar]);
		textMesh.text = textBuilder.ToString();
		currentChar -= 1;
	}
	
	void RemoveText ()
	{
		// if currentChar is 0, we've added all our text to the screen. cancel and return.
		if (textBuilder.Length == 0)
		{
			CancelInvoke("RemoveText");
			return;
		}
		// add the next character to the string builder and update the textmesh
		textBuilder.Remove(textBuilder.Length-1,1);
		textMesh.text = textBuilder.ToString();
		currentChar += 1;
	}
	
	void OnReset()
	{
		CancelInvoke();
		currentChar = 0;
		InvokeRepeating ("RemoveText",0.01f,0.05f);
	}
}
