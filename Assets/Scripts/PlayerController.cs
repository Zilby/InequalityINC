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
	public enum Direction
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

	/// <summary>
	/// Whether or not the player is currently moving. 
	/// </summary>
	private bool moving = false;

	/// <summary>
	/// The current character the player is facing. 
	/// Null if there isn't one. 
	/// </summary>
	private Character characterFacing;

	/// <summary>
	/// Gets the direction vector based on the current direction 
	/// the player is facing. 
	/// </summary>
	private Vector3 DirectionVector
	{
		get
		{
			switch (direction)
			{
				case Direction.up:
					return Vector3.up;
				case Direction.right:
					return Vector3.right;
				case Direction.down:
					return Vector3.down;
				case Direction.left:
					return Vector3.left;
				case Direction.none:
				default:
					return Vector3.zero;
			}
		}
	}


	void Awake()
	{
		destination = transform.position;
		cc = GetComponent<CharacterController>();
		ani = GetComponent<Animator>();
		StartCoroutine(Facing());
	}


	void FixedUpdate()
	{
		SetAnimationState();

		Move(DirectionVector);

		CheckTalk();

		Vector3 dir = destination - transform.position;
		// calculate movement at the desired speed:
		Vector3 movement = dir.normalized * speed * Time.deltaTime;
		// limit movement to never pass the target position:
		movement = movement.magnitude > dir.magnitude ? dir : movement;
		// move the character:
		cc.Move(movement);
	}


	/// <summary>
	/// Gets input to determine which direction the player should be facing and whether or not the player is moving. 
	/// Also handles calling the talk function (since you can't do this during the delay). 
	/// </summary>
	private IEnumerator Facing()
	{
		for (;;)
		{
			if (Time.timeScale != 0.0f)
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
					yield return Talk();
				}
			}
			yield return new WaitForEndOfFrame();
		}
	}


	/// <summary>
	/// Sets the state of the animator to either animating or not.
	/// </summary>
	private void SetAnimationState()
	{
		if (destination == transform.position)
		{
			ani.SetInteger("Direction", (int)direction);
		}
		else
		{
			ani.SetInteger("Direction", (int)Direction.none);
		}
	}


	/// <summary>
	/// Causes the player to face the given direction d.
	/// </summary>
	/// <param name="d">The direction to face</param>
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


	/// <summary>
	/// Moves the player in the specified direction vector. 
	/// </summary>
	private void Move(Vector3 d)
	{
		if (moving)
		{
			lastPos = transform.position;
			destination += (d) / 1;
		}
	}


	/// <summary>
	/// Checks if there's currently a character in front of the player to talk to. 
	/// </summary>
	private void CheckTalk()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, DirectionVector, out hit, 1))
		{
			characterFacing = hit.collider.GetComponent<Character>();
		} else {
			characterFacing = null;
		}
	}


	/// <summary>
	/// Talks to whatever character the player is facing. 
	/// </summary>
	private IEnumerator Talk() 
	{
		if(Input.GetKey(KeyCode.Space) && characterFacing != null) {
			characterFacing.Face(direction);
			yield return new WaitForSeconds(0.3f);
			UIManager.StartText(characterFacing.DialogueScene);
			// Delay after talking to avoid accidental second talk. 
			yield return new WaitForSeconds(0.5f);
		}
		yield return null;
	}


	/// <summary>
	/// Causes player collisions to revert player to last position. 
	/// </summary>
	void OnTriggerEnter()
	{
		destination = lastPos;
	}
}
