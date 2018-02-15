using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all in-game sounds.
/// </summary>
public class SoundManager : MonoBehaviour {

	/// <summary>
	/// Event called when the player bumps into something. 
	/// </summary>
	public static Action BumpEvent;

	/// <summary>
	/// The list of audio clips. 
	/// </summary>
	public List<AudioClip> audioClips;

	/// <summary>
	/// The audio source.
	/// </summary>
	private AudioSource aS;


	private void Awake() {
		aS = GetComponent<AudioSource>();
		BumpEvent = Bump;
	}


	/// <summary>
	/// Creates a bump sound effect. 
	/// </summary>
	private void Bump() {
		aS.PlayOneShot(audioClips[0]);
	}
}
