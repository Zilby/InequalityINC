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

	public static Action TextEvent;

	public static Action StopTextEvent;

	public static Action ClickEvent;

	public delegate void SongPlay(int i);
	public static SongPlay SongEvent;

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
		TextEvent = Text;
		StopTextEvent = StopText;
		ClickEvent = Click;
		SongEvent = PlaySong;
	}


	/// <summary>
	/// Creates a bump sound effect. 
	/// </summary>
	private void Bump() {
		aS[0].PlayOneShot(sounds[0]);
	}


	private void Text() 
	{
		aS[2].clip = sounds[1];
		aS[2].Play();
	}

	private void StopText()
	{
		aS[2].Stop();
	}


	/// <summary>
	/// Plays a given song of index i in music.
	/// </summary>
	private void PlaySong(int i) {
		aS[1].clip = music[i];
		aS[1].Play();
	}

	private void Click() {
		aS[0].PlayOneShot(sounds[2]);
	}
}
