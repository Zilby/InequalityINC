using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueOnEnter
{
	public int day;
	public int scene;
	public DialogueManager.Character character;

	public bool Activated {
		get { return activated; }
	}

	private bool activated = false;


	public void StartDialogue()
	{
		DialogueManager.StartText(scene, 9, false, character, false);
		activated = true;
	}
}
