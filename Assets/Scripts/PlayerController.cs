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

	private enum Direction
	{
		up = 0,
		right = 1,
		down = 2,
		left = 3,
		none = 4,
	}

	private Direction direction = Direction.none;

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

	private Animator ani;

	void Awake()
	{
		destination = transform.position;
		cc = GetComponent<CharacterController>();
		ani = GetComponent<Animator>();
	}

	void FixedUpdate()
	{
		direction = Direction.none;
		if (Input.GetKey(KeyCode.UpArrow) && transform.position == destination)
		{
			Move(Vector3.up);
			direction = Direction.up;
		}
		else if (Input.GetKey(KeyCode.RightArrow) && transform.position == destination)
		{
			Move(Vector3.right);
			direction = Direction.right;
		}
		else if (Input.GetKey(KeyCode.DownArrow) && transform.position == destination)
		{
			Move(Vector3.down);
			direction = Direction.down;
		}
		else if (Input.GetKey(KeyCode.LeftArrow) && transform.position == destination)
		{
			Move(Vector3.left);
			direction = Direction.left;
		}
		ani.SetInteger("Direction", (int)direction);

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
	private void Move(Vector3 d) {
		lastPos = transform.position;
		destination += (d) / 1;
	}

	/// <summary>
	/// Causes player collisions to revert player to last position. 
	/// </summary>
	void OnTriggerEnter()
	{
		destination = lastPos;
	}
}
