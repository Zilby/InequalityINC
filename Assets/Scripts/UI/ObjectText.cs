using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectText : MonoBehaviour
{
	/// <summary>
	/// The description texts.
	/// </summary>
	[SerializeField]
	private List<StringListWrapper> descriptionTexts;

	/// <summary>
	/// The character whose info is gathered. 
	/// </summary>
	[SerializeField]
	private DialogueManager.Character characterAffected = DialogueManager.Character.player;

	/// <summary>
	/// The info gathered on the character. 
	/// </summary>
	[SerializeField]
	private int infoGathered = -1;

	public StringListWrapper DescriptionTexts
	{
		get
		{
			GainInfo();
			return descriptionTexts[Random.Range(0, descriptionTexts.Count)];
		}
	}

	/// <summary>
	/// Gains the info associated with this object's text.
	/// </summary>
	public void GainInfo()
	{
		if (characterAffected != DialogueManager.Character.player && infoGathered != -1)
		{
			Stats.hasInfoOn[characterAffected][infoGathered] = true;
			infoGathered = -1;
		}
	}
	/* 
	 * The lounge desk plant is so tiny
	 * I hear it will eventually bud into a flower though
	 * But for now it remains small and unappreciated
	 * Stay strong little desk plant
	 * 
	 * This printer never works
	 * "I'm out of ink" my ass
	 * I JUST saw you get a new cartridge you piece of garbage
	 */
}
