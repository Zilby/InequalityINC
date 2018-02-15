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
	/// The list of sound clips. 
	/// </summary>
	public List<AudioClip> sounds;

	/// <summary>
	/// The list of music clips. 
	/// </summary>
	public List<AudioClip> music;

	/// <summary>
	/// The audio sources.
	/// aS[0] is used to play singleton audio clips.
	/// aS[1] is used to play music. 
	/// </summary>
	private AudioSource[] aS;


	private void Awake() {
		aS = GetComponents<AudioSource>();
		BumpEvent = Bump;
		PlaySong(0);
	}


	/// <summary>
	/// Creates a bump sound effect. 
	/// </summary>
	private void Bump() {
		aS[0].PlayOneShot(sounds[0]);
	}


	/// <summary>
	/// Plays a given song of index i in music.
	/// </summary>
	private void PlaySong(int i) {
		aS[1].clip = music[i];
		aS[1].Play();
	}
}
