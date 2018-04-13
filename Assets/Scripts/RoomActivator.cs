using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates and deactivates rooms depending on the player's location.
/// </summary>
public class RoomActivator : MonoBehaviour
{
	/// <summary>
	/// The top of this room relative to the player. 
	/// </summary>
	public float top;
	/// <summary>
	/// The bottom of this room relative to the player. 
	/// </summary>
	public float bottom;
	/// <summary>
	/// The list of all characters, activates them upon entering their room. 
	/// </summary>
	public List<NPCController> characters;

	/// <summary>
	/// Starts dialogue upon entering a room. 
	/// </summary>
	[SerializeField]
	public List<DialogueOnEnter> dialogueOnEnters;

	/// <summary>
	/// The list of all rooms. 
	/// </summary>
	private static List<RoomActivator> rooms = new List<RoomActivator>();

	private void Awake()
	{
		rooms.Add(this);
	}

	// Update is called once per frame
	void Update()
	{
		float y = PlayerController.GetPosition().y;
		foreach (RoomActivator r in rooms)
		{
			SetActiveState(y, r);
		}
	}

	/// <summary>
	/// Starts a dialogue upon entering a room if it hasn't already been played. 
	/// </summary>
	private void StartDialogue()
	{
		if (Time.timeScale != 0)
		{
			for (int i = 0; i < dialogueOnEnters.Count; ++i)
			{
				DialogueOnEnter d = dialogueOnEnters[i];
				if (!d.Activated && Stats.Day >= d.day)
				{
					d.StartDialogue();
					dialogueOnEnters.Remove(d);
					break;
				}
			}
			foreach (DialogueOnEnter d in dialogueOnEnters)
			{
				if (!d.Activated && Stats.Day == d.day)
				{
					d.StartDialogue();
				}
			}
		}
	}

	/// <summary>
	/// Sets the active state for the given room.
	/// </summary>
	/// <param name="y">The y coordinate of the player.</param>
	/// <param name="r">The room to be activated.</param>
	void SetActiveState(float y, RoomActivator r)
	{
		bool active = y <= r.top && y > r.bottom;
		if(active) {
			StartDialogue();
		}
		r.gameObject.SetActive(active);
		foreach (NPCController n in r.characters)
		{
			bool nRoom = n.CurrentHours.location.y < r.top && 
			                n.CurrentHours.location.y >= r.bottom;
			if (nRoom)
			{
				if (n.PresentInOffice && active)
				{
					n.Fs.Show();
				}
				else
				{
					n.Fs.Hide();
				}
			}
		}
	}

	private void OnDestroy()
	{
		rooms = new List<RoomActivator>();
	}
}
