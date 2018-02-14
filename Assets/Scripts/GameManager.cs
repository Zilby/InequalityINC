﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// Manages gameplay aspects of the main game. 
/// </summary>
public class GameManager : MonoBehaviour
{

	/// <summary>
	/// Causes the gameplay to pause. 
	/// </summary>
	public static Action PauseEvent;


	void Awake()
	{
		PauseEvent = Pause;
		Stats.ResetAll();
	}


	private void Pause()
	{
		Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;
		Camera.main.GetComponent<BlurOptimized>().enabled = Time.timeScale == 0.0f;
	}
}
