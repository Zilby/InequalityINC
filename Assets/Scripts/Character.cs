using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An individual character within the game. 
/// </summary>
public class Character : MonoBehaviour {

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
	/// The dialogue scene for talking to this character a second time. 
	/// Default value is -1 for if it's the same dialogue.
	/// </summary>
	[SerializeField]
	private int secondTalk = -1;

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

	/// <summary>
	/// Causes the character to face the given direction. 
	/// </summary>
	public void Face(PlayerController.Direction d) {
		rend.sprite = sprites[(int)d];
	}
}
