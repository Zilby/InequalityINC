using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player character. 
/// </summary>
public class PlayerController : MonoBehaviour
{
	/// <summary>
	/// The speed of the player. 
	/// </summary>
	public float speed = 2.0f;

	/// <summary>
	/// The direction of the player. 
	/// </summary>
	private enum Direction
	{
		up = 0,
		right = 1,
		down = 2,
		left = 3,
		none = 4,
	}

	/// <summary>
	/// The current direction the player is facing
	/// </summary>
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

	/// <summary>
	/// The player's animator. 
	/// </summary>
	private Animator ani;

	private bool moving = false;

	void Awake()
	{
		destination = transform.position;
		cc = GetComponent<CharacterController>();
		ani = GetComponent<Animator>();
		StartCoroutine(Facing());
	}

	private IEnumerator Facing()
	{
		for (;;)
		{
			if (Input.GetKey(KeyCode.UpArrow) && transform.position == destination)
			{
				yield return Face(Direction.up);
			}
			else if (Input.GetKey(KeyCode.RightArrow) && transform.position == destination)
			{
				yield return Face(Direction.right);
			}
			else if (Input.GetKey(KeyCode.DownArrow) && transform.position == destination)
			{
				yield return Face(Direction.down);
			}
			else if (Input.GetKey(KeyCode.LeftArrow) && transform.position == destination)
			{
				yield return Face(Direction.left);
			}
			else
			{
				moving = false;
			}
			ani.SetInteger("Direction", (int)direction);
			if (destination == transform.position)
			{
				ani.StopPlayback();
			}
			yield return new WaitForEndOfFrame();
		}
	}


	private IEnumerator Face(Direction d)
	{
		float turnDelay = 0.03f;
		if (direction == d)
		{
			moving = true;
		}
		else
		{
			yield return new WaitForSecondsRealtime(turnDelay);
		}
		direction = d;
	}


	void FixedUpdate()
	{
		if (moving)
		{
			switch (direction)
			{
				case Direction.up:
					Move(Vector3.up);
					break;
				case Direction.right:
					Move(Vector3.right);
					break;
				case Direction.down:
					Move(Vector3.down);
					break;
				case Direction.left:
					Move(Vector3.left);
					break;
			}
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
	private void Move(Vector3 d)
	{
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
