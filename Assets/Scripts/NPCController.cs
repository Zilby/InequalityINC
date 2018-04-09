using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An individual character within the game. 
/// </summary>
public class NPCController : MonoBehaviour
{
	/// <summary>
	/// Causes characters to arrive and leave. 
	/// </summary>
	public static Action ArriveLeave;

	public static Action Relocate;

	public DialogueManager.Character character;

	/// <summary>
	/// List of sprites for this character. 
	/// Should be ordered such that they correspond to facing the player character. 
	/// down = 0,
	/// left = 1,
	///	up = 2,
	///	right = 3,
	/// </summary>
	public List<Sprite> sprites = new List<Sprite>(4);

	/// <summary>
	/// The positive dialogues for this character.
	/// </summary>
	public List<int> positiveDialogues;

	/// <summary>
	/// The negative dialogues for this character.
	/// </summary>
	public List<int> negativeDialogues;

	/// <summary>
	/// The positive dialogue snippets for this character.
	/// </summary>
	public List<int> positiveSnippets;

	/// <summary>
	/// The negative dialogue snippets for this character.
	/// </summary>
	public List<int> negativeSnippets;

	public int conversationsRemaining = 1;

	public List<Hours> hours = new List<Hours>(5);

	[Serializable]
	public class Hours
	{
		public int arrive = 9 * 60;

		public int leave = 17 * 60;

		public Vector3 location;
	}

	public bool PresentInOffice
	{
		get
		{
			return !Stats.fired[character] &&
						 Stats.CurrentTime >= CurrentHours.arrive &&
						 Stats.CurrentTime < CurrentHours.leave;
		}
	}

	public bool NoAvailableDialogue
	{
		get
		{
			int index = Stats.dialogueIndex[character];
			return positiveDialogues.Count <= index && negativeDialogues.Count <= index;
		}
	}

	public Hours CurrentHours
	{
		get { return hours[Stats.Day]; }
	}

	public FadeableSprite Fs
	{
		get { return fs; }
	}

	/// <summary>
	/// The image component of this character. 
	/// </summary>
	private SpriteRenderer rend;

	/// <summary>
	/// The fadeable sprite component of this character. 
	/// </summary>
	private FadeableSprite fs;

	private void Awake()
	{
		rend = GetComponent<SpriteRenderer>();
		fs = GetComponent<FadeableSprite>();
		ArriveLeave += FadeInOut;
		Relocate += SetLocation;
	}


	private void Start()
	{
		if (CurrentHours.arrive > 9 * 60)
		{
			fs.Hide();
		}
		else if (!Stats.fired[character])
		{
			fs.Show();
		}
	}


	private void FadeInOut()
	{
		if (!PresentInOffice && fs.IsVisible)
		{
			fs.SelfFadeOut();
		}
		if (PresentInOffice && !fs.IsVisible)
		{
			fs.SelfFadeIn();
		}
	}


	/// <summary>
	/// The dialogue scene when talking to this character.
	/// </summary>
	public int DialogueScene
	{
		get
		{
			int index = Stats.dialogueIndex[character];
			bool goodTerms = Stats.relationshipPoints[character] >= 0;
			if (conversationsRemaining > 0 && !NoAvailableDialogue)
			{
				conversationsRemaining--;
				Stats.dialogueIndex[character]++;
				return goodTerms ? positiveDialogues[index] : negativeDialogues[index];
			}
			else
			{
				return goodTerms ? positiveSnippets[index - 1] : negativeSnippets[index - 1];
			}
		}
	}

	/// <summary>
	/// Causes the character to face the given direction. 
	/// </summary>
	public void Face(PlayerController.Direction d)
	{
		rend.sprite = sprites[(int)d];
	}


	private void SetLocation()
	{
		transform.localPosition = CurrentHours.location;
	}

	private void OnDestroy()
	{
		ArriveLeave = null;
		Relocate = null;
	}
}
