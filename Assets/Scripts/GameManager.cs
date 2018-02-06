using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			UIManager.StartText(0);
		}
	}


	private void Pause()
	{
		Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;
		UIManager.PauseEvent();
	}
}
