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
	/// Event for updating the clock. 
	/// </summary>
	public static Action ClockEvent;

	/// <summary>
	/// The background for a dialogue scene
	/// </summary>
	public FadeableUI overlay;

	/// <summary>
	/// The clock text.
	/// </summary>
	public TextMeshProUGUI clockText;

	/// <summary>
	/// Causes the UI to show the pause display. 
	/// </summary>
	public static Action PauseEvent;


	// Use this for initialization
	void Awake()
	{
		PauseEvent = Pause;
		ClockEvent = UpdateClock;
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
}
