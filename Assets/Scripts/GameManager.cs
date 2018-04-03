using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

/// <summary>
/// Manages gameplay aspects of the main game. 
/// </summary>
public class GameManager : MonoBehaviour
{
	public GameObject characterHolder;
	private NPCController[] characters;

	private const int TIME_INCREMENT_DELAY = 30;

	/// <summary>
	/// Causes the gameplay to pause. 
	/// </summary>
	public static Action PauseEvent;


	void Awake()
	{
		PauseEvent = Pause;
		//characters = characterHolder.GetComponentsInChildren<NPCController>();
	}


	void Start()
	{
		Stats.ResetAll();
		//StartCoroutine(NewDay());
	}


	private void Update()
	{
		NPCController.ArriveLeave();
	}


	private void Pause()
	{
		Time.timeScale = Time.timeScale == 0.0f ? 1.0f : 0.0f;
		Camera.main.GetComponent<BlurOptimized>().enabled = Time.timeScale == 0.0f;
	}


	/// <summary>
	/// Increments the time per minute of in-game time.
	/// </summary>
	private IEnumerator IncrementTime()
	{
		while (Stats.CurrentTime < 17 * 60)
		{
			yield return new WaitForSeconds(TIME_INCREMENT_DELAY);
			if (Stats.CurrentTime < 17 * 60)
			{
				Stats.CurrentTime += 10;
			}
		}
		yield return null;
		StartCoroutine(NewDay());
	}

	public void OnApplicationQuit()
	{
		Logger.Log("Application Quit");
	}

	/// <summary>
	/// Sets up a new day
	/// </summary>
	private IEnumerator NewDay()
	{
		//Pause();
		//Stats.ResetDay();
		//Stats.Day++;
		//PlayerController.ResetEvent();
		//yield return UIManager.FadeDayEvent();
		yield return null;
		if (Stats.Day == 1)
		{
			// Day 1 exclusive content
		}
		if (Stats.Day == 2)
		{
			// Day 2 exclusive content
		}
		if (Stats.Day == 3)
		{
			// Day 3 exclusive content
		}
		if (Stats.Day == 4)
		{
			// Day 4 exclusive content
		}
		if (Stats.Day == 5)
		{
			// Day 5 exclusive content
		}
		//Pause();
		//StartCoroutine(IncrementTime());
	}
}
