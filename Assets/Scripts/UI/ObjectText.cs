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

	public StringListWrapper DescriptionTexts
	{
		get
		{
			return descriptionTexts[Random.Range(0, descriptionTexts.Count)];
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
