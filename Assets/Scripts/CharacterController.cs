using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An individual character within the game. 
/// </summary>
public class CharacterController : MonoBehaviour
{

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
	/// The dialogue scene when talking to this character.
	/// </summary>
	[SerializeField]
	private int dialogueScene;

	/// <summary>
	/// The dialogue scene for talking to this character a second time
	/// after a positive interaction.  
	/// Default value is -1 for if it's the same dialogue.
	/// </summary>
	public int secondTalkPositive = -1;

	/// <summary>
	/// The dialogue scene for talking to this character a second time
	/// after a negative interaction.  
	/// Default value is -1 for if it's the same dialogue.
	/// </summary>
	public int secondTalkNegative = -1;

	/// <summary>
	/// The dialogue scene for talking to this character a second time. 
	/// Default value is -1 for if it's the same dialogue.
	/// </summary>
	[SerializeField]
	private int secondTalkSnippet = -1;

	/// <summary>
	/// The image component of this character. 
	/// </summary>
	private SpriteRenderer rend;

	private void Awake()
	{
		rend = GetComponent<SpriteRenderer>();
	}

	/// <summary>
	/// The dialogue scene when talking to this character.
	/// </summary>
	public int DialogueScene
	{
		get
		{
			int s = dialogueScene;
			if (secondTalkSnippet > 0)
			{
				dialogueScene = secondTalkSnippet;
			}
			return s;
		}
		set
		{
			dialogueScene = value;
		}
	}

	/// <summary>
	/// Causes the character to face the given direction. 
	/// </summary>
	public void Face(PlayerController.Direction d)
	{
		rend.sprite = sprites[(int)d];
	}
}
