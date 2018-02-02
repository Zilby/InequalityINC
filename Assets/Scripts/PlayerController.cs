using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player character. 
/// </summary>
public class PlayerController : MonoBehaviour {
	/// <summary>
	/// The speed of the player. 
	/// </summary>
	public float speed = 2.0f;

	/// <summary>
	/// The current destination of the player. 
	/// </summary>
	private Vector3 destination;

	/// <summary>
	/// The last grid position the player was in. 
	/// </summary>
	private Vector3 lastPos;

	/// <summary>
	/// The player's character controller. 
	/// </summary>
	private CharacterController cc;

	void Awake()
	{
		destination = transform.position;
		cc = GetComponent<CharacterController>();
	}

	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.UpArrow) && transform.position == destination)
		{
			Move(Vector3.up);
		}
		else if (Input.GetKey(KeyCode.RightArrow) && transform.position == destination)
		{
			Move(Vector3.right);
		}
		else if (Input.GetKey(KeyCode.DownArrow) && transform.position == destination)
		{
			Move(Vector3.down);
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && transform.position == destination)
		{
			Move(Vector3.left);
		}

		Vector3 dir = destination - transform.position;
		// calculate movement at the desired speed:
		Vector3 movement = dir.normalized * speed * Time.deltaTime;
		// limit movement to never pass the target position:
		movement = movement.magnitude > dir.magnitude ? dir : movement;
		// move the character:
		cc.Move(movement);
	}

	/// <summary>
	/// Moves the player in the specified direction vector. 
	/// </summary>
	private void Move(Vector3 direction) {
		lastPos = transform.position;
		destination += (direction) / 1;
	}

	/// <summary>
	/// Causes player collisions to revert player to last position. 
	/// </summary>
	void OnTriggerEnter()
	{
		destination = lastPos;
	}
}
