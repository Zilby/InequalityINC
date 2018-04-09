using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivator : MonoBehaviour
{

	public float top;
	public float bottom;
	public List<NPCController> characters;

	[SerializeField]
	public List<DialogueOnEnter> dialogueOnEnters;

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

	private void OnEnable()
	{
		foreach(DialogueOnEnter d in dialogueOnEnters) {
			if(!d.Activated && Stats.Day == d.day) {
				d.StartDialogue();
			}
		}
	}

	void SetActiveState(float y, RoomActivator r)
	{
		bool active = y <= r.top && y > r.bottom;
		r.gameObject.SetActive(active);
		foreach (NPCController n in r.characters)
		{
			bool nRoom = n.CurrentHours.location.y <= r.top && 
			                n.CurrentHours.location.y > r.bottom;
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
