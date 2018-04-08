using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomActivator : MonoBehaviour {

	public float top;
	public float bottom;
	public List<GameObject> characters;

	private static List<RoomActivator> rooms = new List<RoomActivator>();

	private void Awake()
	{
		rooms.Add(this);
	}

	// Update is called once per frame
	void Update () {
		float y = PlayerController.GetPosition().y;
		foreach(RoomActivator r in rooms) {
			SetActiveState(y, r);
		}
	}

	void SetActiveState(float y, RoomActivator r) {
		bool active = y <= r.top && y > r.bottom;
		r.gameObject.SetActive(active);
		foreach (GameObject g in r.characters) {
			g.SetActive(active);
		}
	}
}
