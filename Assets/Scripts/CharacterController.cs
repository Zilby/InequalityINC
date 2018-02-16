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
			int index = Stats.dialogueIndex[character];
			bool goodTerms = Stats.relationshipPoints[character] >= 0;
			if (conversationsRemaining > 0)
			{
				conversationsRemaining--;
				Stats.dialogueIndex[character]++;
				return goodTerms ? positiveDialogues[index] : negativeDialogues[index];
			}
			else
			{
				return goodTerms ? positiveSnippets[index] : negativeSnippets[index];
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
}
