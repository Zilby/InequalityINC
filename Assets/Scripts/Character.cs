using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An individual character within the game. 
/// </summary>
public class Character : MonoBehaviour {
	
	/// <summary>
	/// The dialogue scene when talking to this character.
	/// </summary>
	[SerializeField]
	private int dialogueScene;

	/// <summary>
	/// The dialogue scene for talking to this character a second time. 
	/// Default value is -1 for if it's the same dialogue.
	/// </summary>
	[SerializeField]
	private int secondTalk = -1;

	public int DialogueScene {
		get
		{
			int s = dialogueScene;
			if (secondTalk > 0)
			{
				dialogueScene = secondTalk;
			}
			return s;
		}
	}
}
