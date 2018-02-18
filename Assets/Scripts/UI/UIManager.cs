using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the main UI. 
/// </summary>
public class UIManager : MonoBehaviour
{
	/// <summary>
	/// Event for updating the description text. 
	/// </summary>
	public delegate IEnumerator TextEvent(string s);
	/// <summary>
	/// Updates the description text. 
	/// </summary>
	public static TextEvent UpdateText;

	/// <summary>
	/// Event for updating the clock. 
	/// </summary>
	public static Action ClockEvent;

	/// <summary>
	/// Causes the UI to show the pause display. 
	/// </summary>
	public static Action PauseEvent;

	/// <summary>
	/// Causes the UI to show the text display. 
	/// </summary>
	public static Action DescripEvent;

	/// <summary>
	/// The background for all UI elements
	/// </summary>
	public FadeableUI overlay;

	/// <summary>
	/// The fadeable UI for descriptions. 
	/// </summary>
	public FadeableUI description;

	/// <summary>
	/// The description text.
	/// </summary>
	public MoveableText descripText;

	/// <summary>
	/// The clock text.
	/// </summary>
	public TextMeshProUGUI clockText;


	// Use this for initialization
	void Awake()
	{
		PauseEvent = Pause;
		ClockEvent = UpdateClock;
		DescripEvent = UpdateDescrip;
		UpdateText = SetText;
		description.Hide();
	}


	/// <summary>
	/// Used when the game pauses. 
	/// </summary>
	private void Pause()
	{
		if (Time.timeScale == 0.0f)
		{
			overlay.SelfFadeIn();
		}
		else
		{
			overlay.SelfFadeOut();
		}
	}


	/// <summary>
	/// Updates the clock.
	/// </summary>
	private void UpdateClock()
	{
		clockText.text = Stats.GetTime();
	}


	/// <summary>
	/// Updates the description.
	/// </summary>
	private void UpdateDescrip()
	{
		descripText.ClearText();
		if (description.IsVisible)
		{
			description.SelfFadeOut();
		}
		else
		{
			description.SelfFadeIn();
		}
	}


	/// <summary>
	/// Sets the text for the given string.
	/// </summary>
	private IEnumerator SetText(string s)
	{
		descripText.ClearText();
		yield return descripText.TypeText(s);
		yield return new WaitForSecondsRealtime(0.1f);
		yield return DialogueManager.WaitForKeypress(KeyCode.Space);
	}
}
