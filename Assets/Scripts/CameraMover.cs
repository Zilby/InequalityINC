using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the cameras with the player.
/// </summary>
public class CameraMover : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		transform.position = PlayerController.GetPosition();
	}
}
